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
        private readonly IAddressEngineService _addressLookup;

        public WarehouseAddEditModel( IVendorWarehouseService warehouseService,IAddressEngineService addressService)
        {
            _warehouseService = warehouseService;
            _addressLookup = addressService;
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
            AddressList = await _addressLookup.GetAddressesLookupAsync("WAREHOUSE");

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
                AddressList = await _addressLookup.GetAddressesLookupAsync("WAREHOUSE");
                return Page();
            }

            await _warehouseService.SaveWarehouseAsync(Warehouse);

            TempData["SuccessMessage"] = "Warehouse saved successfully";

            return RedirectToPage("/Warehouse/List");
        }

        
    }
}