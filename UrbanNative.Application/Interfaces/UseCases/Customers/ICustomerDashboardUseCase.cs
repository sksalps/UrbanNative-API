using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Customers;

namespace UrbanNative.Application.Interfaces.UseCases.Customers
{
    public interface ICustomerDashboardUseCase
    {
        Task<CustomersDashboardDto> GetDashboardAsync(int userId);
    }
}
