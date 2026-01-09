using UrbanNative.Admin.Models;
using UrbanNative.Application.DTOs.AdminOrders;

namespace UrbanNative.Admin.Services
{
    public interface IAdminOrderService
    {
        Task<AdminOrderPagedResultDto> GetOrdersAsync(OrderFilterModel filter       );
        Task<AdminOrderDetailsDto> GetOrderDetailsAsync(int orderId);
        Task<AdminOrderShipmentDetailsDto> GetOrderShipmentDetailsAsync(int orderId);
        Task<byte[]> DownloadOrderPdfAsync(int orderId);
    }
}
