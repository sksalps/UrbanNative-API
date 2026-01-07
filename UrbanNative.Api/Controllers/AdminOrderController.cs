using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces;

[ApiController]
[Route("api/admin/orders")]
public class AdminOrderController : ControllerBase
{
    private readonly IAdminOrderRepository _repository;

    public AdminOrderController(IAdminOrderRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
    DateTime? fromDate,
    DateTime? toDate,
    string? orderStatus)
    {
        var result = await _repository.GetOrdersAsync(fromDate, toDate, orderStatus);
        return Ok(result);
    }


    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderDetails(int orderId)
    {
        var result = await _repository.GetOrderDetailsAsync(orderId);
        return Ok(result);
    }
}
