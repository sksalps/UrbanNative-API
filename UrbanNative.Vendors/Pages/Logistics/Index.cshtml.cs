using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors.Logistics;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Logistics
{
    public class IndexModel : PageModel
    {
        private readonly IVendorLogisticsService _service;
        private readonly ISkuFilterService _commonService;

        public IndexModel(
            IVendorLogisticsService service,
            ISkuFilterService commonService)
        {
            _service = service;
            _commonService = commonService;
        }

        /* ================= FILTER ================= */

        [BindProperty]
        public VendorLogisticsFilterDto Filter { get; set; } = new();

        /* ================= GRID-1 ================= */

        public IReadOnlyList<VendorLogisticsOrderDto> Orders { get; set; }
            = new List<VendorLogisticsOrderDto>();
        

        /* ================= GRID-2 ================= */

        public IReadOnlyList<VendorLogisticsItemDto> DispatchItems { get; set; }
            = new List<VendorLogisticsItemDto>();

        [BindProperty]
        public List<int> SelectedOrderItemIds { get; set; } = new();

        [BindProperty]
        public int DispatchOrderId { get; set; }
        
        //[BindProperty]
        //public String OrderNo { get; set; } =  string.Empty;
        
        [BindProperty]
        public int DispatchCity { get; set; } =new();

        /* ================= GRID-3 ================= */

        public IReadOnlyList<VendorLogisticsShipmentDto> Shipments { get; set; }
            = new List<VendorLogisticsShipmentDto>();

        /* ================= MODAL ================= */
        [BindProperty]
        public CreateVendorShipmentDto CreateShipment { get; set; }


        [BindProperty]
        public UpdateShipmentStatusDto UpdateShipment { get; set; } = new();

        public IReadOnlyList<LogisticsProviderComnDto> LogisticsProviders { get; set; }
            = new List<LogisticsProviderComnDto>();

        /* ============================================================
         * INITIAL LOAD
         * ============================================================ */
        public async Task OnGetAsync()
        {
            LogisticsProviders = await _commonService.GetLogisticsProvidersAsync();
            
            Orders = await _service.GetOrdersAsync(showCompleted: false);
            //DispatchCity = Orders.CityState;
            //Shipments = await _service.GetShipmentsAsync( orderId: null, showCompleted: false);
        }

        /* ============================================================
         * SEARCH / FILTER
         * ============================================================ */
        public async Task<IActionResult> OnPostSearchAsync()
        {
            LogisticsProviders = await _commonService.GetLogisticsProvidersAsync();

            Orders = await _service.GetOrdersAsync(Filter.ShowCompleted, Filter.SearchText);

            Shipments = await _service.GetShipmentsAsync(orderId: null,showCompleted: Filter.ShowCompleted);

            DispatchItems = new List<VendorLogisticsItemDto>(); // reset Grid-2
            return Page();
        }

        /* ============================================================
         * GRID-1 PARTIAL RELOAD
         * ============================================================ */
        public async Task<IActionResult> OnGetOrdersGridAsync()
        {
            Orders = await _service.GetOrdersAsync(Filter.ShowCompleted);
            
            // Clear citystate column as it's used for dispatch city selection in UI
            return Partial("_OrdersGrid1", this);

        }

        /* ============================================================
         * GRID-2 LOAD (ON ORDER UPDATE CLICK)
         * ============================================================ */
        
        public async Task<IActionResult> OnGetItemsAsync(int orderId)
        {
            DispatchOrderId = orderId;
            
            DispatchItems = await _service.GetOrderItemsAsync(orderId);

            return Partial("_DispatchItemGrid2", this);
            

        }

        


        /* ============================================================
         * GRID-3 PARTIAL LOAD
         * ============================================================ */
        public async Task<IActionResult> OnGetShipmentsAsync(
            int? orderId,
            bool showCompleted = false)
        {
            Shipments = await _service.GetShipmentsAsync(orderId, showCompleted);
            return Partial("_ShipmentGrid3", this);
        }
        /* ============================================================
         * CREATE SHIPMENT (FROM GRID-2)
         * ============================================================ */
        public async Task<IActionResult> OnPostCreateShipmentAsync(CreateVendorShipmentDto dto)
        {
            if (dto.OrderItemIds == null || !dto.OrderItemIds.Any())
                return BadRequest("No items selected for dispatch");

            await _service.CreateShipmentAsync(dto);

            return new JsonResult(new { success = true });
        }
        /* ============================================================
         * UPDATE SHIPMENT STATUS (MODAL)
         * ============================================================ */
        public async Task<IActionResult> OnPostUpdateShipmentAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid shipment data");

            await _service.UpdateShipmentStatusAsync(UpdateShipment);

            return new JsonResult(new { success = true });
        }
    }
}
