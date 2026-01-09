using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.AdminOrders;


namespace UrbanNative.Application.Interfaces
{
    
public interface IAdminOrderRepository
{
        Task<AdminOrderPagedResultDto> GetOrdersAsync(string orderNo, DateTime? fromDate,DateTime? toDate,string paymentStatus,
                string orderStatus,
                int? userId,
                int? vendorId,
                int pageNumber,
                int pageSize);

    Task<AdminOrderDetailsDto> GetOrderDetailsAsync(int orderId);
    Task<AdminOrderShipmentDetailsDto> GetOrderShipmentDetailsAsync(int orderId);

 
}


}
