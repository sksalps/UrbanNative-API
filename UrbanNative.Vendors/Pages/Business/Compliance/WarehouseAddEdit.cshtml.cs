using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Common;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages
{
    public class WarehouseAddEditModel : PageModel
    {
        private readonly IVendorWarehouseService _warehouseService;
        private readonly IAddressEngineService _addressService;

        public WarehouseAddEditModel( IVendorWarehouseService warehouseService,IAddressEngineService addressService)
        {
            _warehouseService = warehouseService;
            _addressService = addressService;
        }

        // 🔹 Bind Warehouse
        [BindProperty]
        public VendorWarehouseSaveDto Warehouse { get; set; }

        // 🔹 Address Dropdown
        public List<AddressListDto> AddressList { get; set; } = new();

        // 🔹 Mode
        public bool IsEditMode => Warehouse?.WarehouseId != null;

        // 🔷 GET
        public async Task OnGetAsync(int? id)
        {
            // Load Address List
            AddressList = await _addressService.GetAddressesAsync("WAREHOUSE");

            if (id.HasValue)
            {
                Warehouse = await _warehouseService.GetWarehouseByIdAsync(id.Value);
            }
            else
            {
                Warehouse = new VendorWarehouseSaveDto();
            }
        }

        // 🔷 POST (Save Warehouse)
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AddressList = await _addressService.GetAddressesAsync("WAREHOUSE");
                return Page();
            }

            await _warehouseService.SaveWarehouseAsync(Warehouse);

            TempData["SuccessMessage"] = "Warehouse saved successfully";

            return RedirectToPage("/Warehouse/List");
        }

        // ============================================================
        // 🔷 ADDRESS HANDLERS (MODAL)
        // ============================================================

        // 🔹 Get Address List
        public async Task<JsonResult> OnGetAddressLookupAsync(string type)
        {
            var data = await _addressService.GetAddressesAsync(type);
            return new JsonResult(data);
        }

        // 🔹 Get Address By ID (Edit Mode)
        public async Task<JsonResult> OnGetAddressByIdAsync(int addressId)
        {
            var data = await _addressService.GetByIdAsync(addressId);
            return new JsonResult(data);
        }

        // 🔹 Save Address
        public async Task<JsonResult> OnPostSaveAddressAsync([FromBody] AddressSaveDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.AddressLine1))
                    return new JsonResult(new { success = false, message = "Address Line 1 is required" });

                if (string.IsNullOrWhiteSpace(dto.Pincode))
                    return new JsonResult(new { success = false, message = "Pincode is required" });

                var id = await _addressService.SaveAsync(dto);

                return new JsonResult(new
                {
                    success = true,
                    addressId = id,
                    message = "Address saved successfully"
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}