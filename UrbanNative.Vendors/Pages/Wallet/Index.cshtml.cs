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

        [BindProperty(SupportsGet = true)]
        public WalletLedgerFilterDto Filter { get; set; } = new();

        public async Task OnGetAsync()
        {
            AccountHeads = (await _service.GetAccountHeadsAsync()).ToList();
            Rows = (await _service.GetLedgerAsync(Filter)).ToList();
        }
    }
}
