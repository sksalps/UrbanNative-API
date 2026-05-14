using System.Net.Http.Headers;
using System.Text.Json;
using UrbanNative.Application.DTOs.Customers;
using UrbanNative.Customers.Services.Interfaces;


namespace UrbanNative.Customers.Services
{
    
    public class CustomerDashboardService : ICustomerDashboardService
    {
        private readonly HttpClient _httpFactory;

        public CustomerDashboardService(IHttpClientFactory factory)
        {
            _httpFactory = factory.CreateClient("ApiClient");
        }
        

        public async Task<CustomersDashboardDto> GetDashboardAsync()
        {
            

            var response = await _httpFactory.GetAsync("api/customer/dashboard");

            if (!response.IsSuccessStatusCode)
            {
                // Handle error (log / fallback)
                return new CustomersDashboardDto();
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CustomersDashboardDto>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CustomersDashboardDto();
        }
    }
}
