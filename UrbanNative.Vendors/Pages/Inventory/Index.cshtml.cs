using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly IVendorInventoryService _inventoryService;

        public IndexModel(IVendorInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [BindProperty(SupportsGet = true)]
        public int SkuId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AddressId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime ToDate { get; set; }

        public VendorInventorySummaryDto Summary { get; set; }
            = new();

        public IReadOnlyList<VendorInventoryLogDto> Logs { get; set; }
            = new List<VendorInventoryLogDto>();

        public async Task OnGetAsync(int page = 1)
        {
            if (FromDate == default)
                FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            if (ToDate == default)
                ToDate = DateTime.Today;

            if (SkuId <= 0 || AddressId <= 0)
                return;

            try
            {
                Summary = await _inventoryService.GetInventorySummaryAsync(
                    SkuId, AddressId, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var inner = ex.InnerException?.Message;
                throw;
            }


            Logs = await _inventoryService.GetInventoryLogsAsync(
                SkuId, AddressId, FromDate, ToDate, page, 20);
        }
    }
}
