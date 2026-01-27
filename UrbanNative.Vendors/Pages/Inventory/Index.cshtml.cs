using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly IVendorInventoryService _inventoryService;
        private readonly IVendorProductService _service;

        public IndexModel(IVendorInventoryService inventoryService, IVendorProductService service)
        {
            _inventoryService = inventoryService;
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public int SkuId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? AddressId { get; set; }


        [BindProperty(SupportsGet = true)]
        public DateTime FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime ToDate { get; set; }
        public List<SelectListItem> WarehouseList { get; set; } = new();
        public VendorInventorySummaryDto Summary { get; set; }
            = new();

        public IReadOnlyList<VendorInventoryLogDto> Logs { get; set; }
            = new List<VendorInventoryLogDto>();

        public async Task OnGetAsync(int page = 1)
        {
            // 1️⃣ Date defaults
            if (FromDate == default)
                FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            if (ToDate == default)
                ToDate = DateTime.Today;

            // 2️⃣ SKU must exist
            if (SkuId <= 0)
                return;

            // 3️⃣ Load warehouses ALWAYS
            WarehouseList = await _service.GetVendorWarehousesAsync();

            // Add "All" option at top
            WarehouseList.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "All Warehouses"
            });
          

            if (AddressId == null)
            {
                // Resolve PRIMARY warehouse (preferred)
                var primary = WarehouseList
                    .FirstOrDefault(x => x.Value != "0" && x.Text.Contains("Primary"));

                if (primary != null)
                    AddressId = int.Parse(primary.Value);
                else
                    AddressId = int.Parse(WarehouseList.First(x => x.Value != "0").Value);
            }


            // 5️⃣ Load summary & logs
            Summary = await _inventoryService.GetInventorySummaryAsync(
                SkuId,
                AddressId,   // 0 = All, >0 = specific warehouse, null= Primary WH
                FromDate,
                ToDate);

            Logs = await _inventoryService.GetInventoryLogsAsync(
                SkuId,
                AddressId,
                FromDate,
                ToDate,
                page,
                20);
        }


    }
}
