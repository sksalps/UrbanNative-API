using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.UseCases;

[ApiController]
[Route("api/vendors/warehouses")]
public class VendorWarehousesController : ControllerBase
{
    private readonly IVendorProductsUseCase  _useCase;

    public VendorWarehousesController(IVendorProductsUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        int vendorId = int.Parse(User.FindFirst("VendorId")?.Value);
        var data = await _useCase.ExecuteAsync(vendorId);
        return Ok(data);
    }
}
