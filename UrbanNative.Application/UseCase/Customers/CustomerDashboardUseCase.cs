using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Customers;
using UrbanNative.Application.Interfaces.Customers;
using UrbanNative.Application.Interfaces.UseCases.Customers;

namespace UrbanNative.Application.UseCase.Customers
{
   

    public class CustomerDashboardUseCase : ICustomerDashboardUseCase
    {
        private readonly ICustomerDashboardRepository _repository;

        public CustomerDashboardUseCase(ICustomerDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomersDashboardDto> GetDashboardAsync(int userId)
        {
            // 🔷 Basic validation (important)
            if (userId <= 0)
                throw new ArgumentException("Invalid User ID");

            // 🔷 Call repository
            var dashboard = await _repository.GetDashboardAsync(userId);

            // 🔷 Future hooks (VERY IMPORTANT DESIGN)
            // - Add wallet enrichment
            // - Add AI recommendations
            // - Add affiliate logic
            // - Add personalization

            return dashboard;
        }
    }
}
