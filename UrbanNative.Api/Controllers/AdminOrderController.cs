using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.PDFdownload;

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
    string? orderNo,
    DateTime? fromDate,
    DateTime? toDate,
    string? paymentStatus,
    string? orderStatus,
    int? userId,
    int? vendorId,
    int pageNumber = 1,
    int pageSize = 20)
    {
        var result = await _repository.GetOrdersAsync(
            orderNo, fromDate, toDate,
            paymentStatus, orderStatus,
            userId, vendorId,
            pageNumber, pageSize
        );

        return Ok(result);
    }



    
    [HttpGet("{orderId}/OrderDetails")]
    public async Task<IActionResult> GetOrderDetails(int orderId)
    {
        var result = await _repository.GetOrderDetailsAsync(orderId);
        return Ok(result);
    }

    [HttpGet("{orderId}/pdf")] // Endpoint to download order details as PDF

    public async Task<IActionResult> DownloadOrderPdf(int orderId)
    {
        var order = await _repository.GetOrderDetailsAsync(orderId);

        var pdfBytes = OrderPdfGenerator.Generate(order);

        return File(
            pdfBytes,
            "application/pdf",
            $"Order_{order.Header.OrderNo}.pdf"
        );
    }



[HttpGet("{orderId}/shipments")]
    public async Task<IActionResult> GetShipmentDetails(int orderId)
    {
        var result = await _repository.GetOrderShipmentDetailsAsync(orderId);
        return Ok(result);
    }
}
