using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminWallet;

namespace UrbanNative.Admin.Pages.Wallet
{
    public class IndexModel : PageModel
    {
        private readonly IAdminWalletService _walletService;

        public IndexModel(IAdminWalletService walletService)
        {
            _walletService = walletService;
        }

        // Filters
        [BindProperty(SupportsGet = true)]
        public string OwnerType { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? OwnerId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string WalletType { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AccHead { get; set; }

        [BindProperty(SupportsGet = true)]
        public string TxnType { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SourceType { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SourceId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }

        public IEnumerable<WalletLedgerDto> Ledger { get; set; } = new List<WalletLedgerDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            // Must have Owner
            if (string.IsNullOrEmpty(OwnerType) || !OwnerId.HasValue)
                return Page();

            Ledger = await _walletService.GetLedgerAsync(
                OwnerType,
                OwnerId.Value,
                WalletType,
                AccHead,
                TxnType,
                SourceType,
                SourceId,
                FromDate,
                ToDate
            );

            return Page();
        }
    }
}
