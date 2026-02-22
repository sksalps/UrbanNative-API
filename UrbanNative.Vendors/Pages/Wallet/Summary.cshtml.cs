using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Wallet
{
    public class SummaryModel : PageModel
    {
        
        private readonly IVendorWalletService _service;

        public SummaryModel(IVendorWalletService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public WalletSummaryFilterDto Filter { get; set; } = new();

        public VendorWalletSummaryReportDto Report { get; set; } = new();

        public List<VendorWalletTypeDto> WalletTypes { get; set; } = new();
        public string DateRangeLabel { get; set; } = "";

        public async Task OnGetAsync()
        {

            SetDefaultFY();
            SetDateRangeLabel();

            WalletTypes = await _service.GetWalletTypesAsync();

            Report = await _service.GetWalletSummaryAsync(Filter);
        }

        public async Task<IActionResult> OnGetExportExcelAsync()
        {
            SetDefaultFY();

            var bytes = await _service.ExportSummaryExcelAsync(Filter);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"WalletSummary_{DateTime.Now:yyyyMMdd}.xlsx");
        }


        public async Task<IActionResult> OnGetExportPdfAsync()
        {
            SetDefaultFY();

            var bytes = await _service.ExportSummaryPdfAsync(Filter);

            return File(bytes, "application/pdf",
                $"WalletSummary_{DateTime.Now:yyyyMMdd}.pdf");
        }

        private void SetDefaultFY()
        {
            if (Filter.FromDate.HasValue && Filter.ToDate.HasValue)
                return;

            var today = DateTime.Today;
            var fyStart = today.Month >= 4
                ? new DateTime(today.Year, 4, 1)
                : new DateTime(today.Year - 1, 4, 1);

            var fyEnd = fyStart.AddYears(1).AddDays(-1);

            Filter.FromDate = fyStart;
            Filter.ToDate = fyEnd;
            Filter.FY ??= "current";
        }

        private void SetDateRangeLabel()
        {
            if (Filter.FY == "current")
            {
                DateRangeLabel = $"Current FY ({GetFYLabel(Filter.FromDate.Value)})";
            }
            else if (Filter.FY == "previous")
            {
                DateRangeLabel = $"Previous FY ({GetFYLabel(Filter.FromDate.Value)})";
            }
            else
            {
                DateRangeLabel = $"Custom Date ({Filter.FromDate:dd MMM yyyy} – {Filter.ToDate:dd MMM yyyy})";
            }
        }

        private string GetFYLabel(DateTime fyStart)
        {
            var fyEndYear = fyStart.AddYears(1).Year;
            return $"{fyStart.Year}-{fyEndYear.ToString().Substring(2)}";
        }

    }
}
