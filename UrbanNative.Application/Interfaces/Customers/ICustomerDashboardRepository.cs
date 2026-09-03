using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Customers;

namespace UrbanNative.Application.Interfaces.Customers
{
    public interface ICustomerDashboardRepository
    {
        Task<CustomersDashboardDto> GetDashboardAsync(int userId);
    }
}
