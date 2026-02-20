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


        [BindProperty(SupportsGet = true)]
        public WalletLedgerFilterDto Filter { get; set; } = new();
        public async Task OnGetAsync()
        {
            if (!Filter.FromDate.HasValue)
                Filter.FromDate = DateTime.Today.AddDays(-30);

            if (!Filter.ToDate.HasValue)
                Filter.ToDate = DateTime.Today;

            AccountHeads = (await _service.GetAccountHeadsAsync()).ToList();
            Rows = await _service.GetLedgerAsync(Filter);
            Summary = await _service.GetLedgerSummaryAsync(Filter);
        }
        public async Task<JsonResult> OnGetOrderSuggestionsAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new JsonResult(new List<string>());

            var results = await _service.SearchOrdersAsync(term);
            return new JsonResult(results);
        }

    }
}
