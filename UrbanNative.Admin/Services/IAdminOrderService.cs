using UrbanNative.Application.DTOs.AdminOrders;

namespace UrbanNative.Admin.Services
{
    public interface IAdminOrderService
    {
        Task<List<AdminOrderListDto>> GetOrdersAsync(
            DateTime? fromDate,
            DateTime? toDate,
            string orderStatus
        );
        Task<AdminOrderDetailsDto> GetOrderDetailsAsync(int orderId);
    }
}
