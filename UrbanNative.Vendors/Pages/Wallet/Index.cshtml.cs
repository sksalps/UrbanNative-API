using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Vendors.Services.Interfaces;


namespace UrbanNative.Vendors.Pages.Wallet
{
    public class IndexModel : PageModel
    {
        private readonly IVendorWalletService _service;

        public IndexModel(IVendorWalletService service)
        {
            _service = service;
        }

        public List<WalletLedgerRowDto> Rows { get; set; } = new();
        public List<AccountHeadDto> AccountHeads { get; set; } = new();
        public WalletLedgerSummaryDto Summary { get; set; } = new();
        public List<VendorWalletTypeDto> WalletTypes { get; set; } = new();
        public int? AccountHeadId { get; set; }

        [BindProperty(SupportsGet = true)]
        public WalletLedgerFilterDto Filter { get; set; } = new();

        public async Task OnGetAsync()
        {
            setDefaultDatesIfEmpty(); // only if null

            WalletTypes = await _service.GetWalletTypesAsync();
            AccountHeads = (await _service.GetAccountHeadsAsync()).ToList();
            Rows = await _service.GetLedgerAsync(Filter);
            Summary = await _service.GetLedgerSummaryAsync(Filter);
        }
        private void setDefaultDatesIfEmpty()
        {
            if (!Filter.FromDate.HasValue)
                Filter.FromDate = DateTime.Today.AddDays(-30);
            if (!Filter.ToDate.HasValue)
                Filter.ToDate = DateTime.Today;

        }

        public async Task<JsonResult> OnGetSmartSearchAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new JsonResult(new List<object>());

            var results = await _service.SmartSearchAsync(term);
            return new JsonResult(results);
        }


    }
}
