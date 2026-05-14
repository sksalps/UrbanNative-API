using UrbanNative.Application.DTOs.Customers;

namespace UrbanNative.Customers.Services.Interfaces
{
    public interface ICustomerDashboardService
    {
        Task<CustomersDashboardDto> GetDashboardAsync();
    }
}
