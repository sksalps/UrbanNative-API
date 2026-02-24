using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Wallet
{ 

    public class SettlementModel : PageModel
    {
        private readonly IVendorSettlementPayoutService _service;

        public SettlementModel(IVendorSettlementPayoutService service)
        {
            _service = service;
        }

        public SettlementPayoutSummaryDto Summary { get; set; } = new();
        public List<SettlementPayoutDto> Settlements { get; set; } = new();
        public List<SettlementPaymentDto> Payments { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string FilterType { get; set; } = "CURRENT_FY";

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public long? SelectedSettlementId { get; set; }

        public async Task OnGetAsync()
        {
            int vendorId = 1; // TODO: use logged-in vendor

            Summary = await _service.GetSummaryAsync();
            Settlements = await _service.GetSettlementsAsync();

            // ✅ Auto-load last settlement
            if (!SelectedSettlementId.HasValue && Settlements.Any())
            {
                SelectedSettlementId = Settlements.First().SettlementID;
            }

            if (SelectedSettlementId.HasValue)
            {
                Payments = await _service.GetPaymentsAsync(SelectedSettlementId.Value);
            }
        }
    }
}