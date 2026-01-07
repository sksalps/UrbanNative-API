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
    Task<List<AdminOrderListDto>> GetOrdersAsync(
        DateTime? fromDate,
        DateTime? toDate,
        string orderStatus
    );

    Task<AdminOrderDetailsDto> GetOrderDetailsAsync(int orderId);
}


}
