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

        public IActionResult OnGetDownloadCsv(
            int skuId,
            DateTime fromDate,
            DateTime toDate)
        {
            if (Report == null || !Report.Logs.Any())
                return BadRequest("No data to export");

            var sb = new StringBuilder();
            sb.AppendLine("Date,ChangeType,Quantity,OldStock,NewStock");

            foreach (var log in Report.Logs)
            {
                sb.AppendLine(
                    $"{log.CreatedAt:yyyy-MM-dd}," +
                    $"{log.ChangeType}," +
                    $"{log.Quantity}," +
                    $"{log.OldStock}," +
                    $"{log.NewStock}"
                );
            }

            return File(
                Encoding.UTF8.GetBytes(sb.ToString()),
                "text/csv",
                $"SKU_{skuId}_InventoryLogs_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.csv"
            );
        }
    }
}
