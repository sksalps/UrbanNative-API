using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminInventory;
using System.Text;

namespace UrbanNative.Admin.Pages.Inventory
{
    public class SkuInventoryLogsModel : PageModel
    {
        private readonly IAdminInventoryLogService _logService;

        public SkuInventoryLogsModel(IAdminInventoryLogService logService)
        {
            _logService = logService;
        }

        // ================= QUERY PARAMS =================

        [BindProperty(SupportsGet = true)]
        public int SkuId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime ToDate { get; set; }

        // ================= RESULT =================

        public AdminInventoryLogPeriodResultDto? Report { get; private set; }

        // ================= PAGE LOAD =================

        public async Task<IActionResult> OnGetAsync()
        {
            // 🔹 Default period = current month
            if (FromDate == default || ToDate == default)
            {
                var now = DateTime.Now;
                FromDate = new DateTime(now.Year, now.Month, 1);
                ToDate = FromDate.AddMonths(1).AddDays(-1);
            }

            Report = await _logService.GetSkuLogsByPeriodAsync(
                SkuId,
                FromDate,
                ToDate
            );

            if (Report == null)
                return NotFound();

            return Page();
        }

        // ================= CSV EXPORT =================

        private static string Escape(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            if (value.Contains(',') || value.Contains('"'))
                return $"\"{value.Replace("\"", "\"\"")}\"";

            return value;
        }


        public async Task<IActionResult> OnGetDownloadCsvAsync( int skuId,    DateTime fromDate,    DateTime toDate)
        {
            if (skuId <= 0)
                return BadRequest("Invalid SKU");


            // 🔹 Always fetch fresh data (CSV is a new request)
            var report = await _logService.GetSkuLogsByPeriodAsync(
                skuId,
                fromDate,
                toDate
            );



            if (report == null || !report.Logs.Any())
                return BadRequest("No data to export");

            var h = report.Header;
            var sb = new StringBuilder();

            // ================= CSV HEADER =================
            sb.AppendLine($"Product,{Escape(h.ProductName)}");
            sb.AppendLine($"Vendor,{Escape(h.VendorName)}");
            sb.AppendLine($"Variant,{Escape(h.VariantDisplay)}");
            sb.AppendLine($"From Date,{fromDate:yyyy-MM-dd}");
            sb.AppendLine($"To Date,{toDate:yyyy-MM-dd}");
            sb.AppendLine($"Opening Stock,{h.OpeningStock}");
            sb.AppendLine($"Closing Stock,{h.ClosingStock}");
            sb.AppendLine(); // empty line

            // ================= TABLE HEADER =================
            sb.AppendLine("Date,ChangeType,Quantity,OldStock,NewStock");

            // ================= DATA =================
            foreach (var log in report.Logs)
            {
                sb.AppendLine(
                    $"{log.CreatedAt:yyyy-MM-dd}," +
                    $"{log.ChangeType}," +
                    $"{log.Quantity}," +
                    $"{log.OldStock}," +
                    $"{log.NewStock}"
                );
            }

            var fileName =
                $"SKU_{skuId}_InventoryLogs_" +
                $"{fromDate:yyyyMMdd}_to_{toDate:yyyyMMdd}.csv";

            return File(
                Encoding.UTF8.GetBytes(sb.ToString()),
                "text/csv",
                fileName
            );
        }


    }
}
