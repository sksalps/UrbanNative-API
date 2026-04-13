using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Vendors.Services;
using UrbanNative.Vendors.Services.Interfaces;

public class WarehouseListModel : PageModel
{

    private readonly IVendorWarehouseService _service;

    public List<VendorWarehouseListDto> Warehouses { get; set; }

    public WarehouseListModel(IVendorWarehouseService service)
    {
        _service = service;
    }

    public async Task OnGetAsync()
    {
        Warehouses = await _service.GetWarehousesAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int warehouseId)
    {
        try
        {
            var result = await _service.DeleteWarehouseAsync(warehouseId);

            return new JsonResult(new
            {
                success = result.IsSuccess,
                message = result.Message
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
    public async Task<IActionResult> OnPostSetPrimaryAsync(int warehouseId)
    {
        try
        {
            var result = await _service.SetPrimaryWarehouseAsync(warehouseId);

            return new JsonResult(new
            {
                success = result.IsSuccess,
                message = result.Message
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