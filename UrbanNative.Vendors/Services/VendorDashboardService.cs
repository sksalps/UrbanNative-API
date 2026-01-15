using System.Net.Http;
using System.Net.Http.Json;
using UrbanNative.Application.DTOs.Vendors;
using static System.Net.WebRequestMethods;

namespace UrbanNative.Vendors.Services
{
    public class VendorDashboardService : IVendorDashboardService
    {
        private readonly HttpClient _httpFactory;

        public VendorDashboardService(IHttpClientFactory factory)
        {
            _httpFactory = factory.CreateClient("ApiClient");
        }

        public Task<VendorDashboardDto> GetDashboardAsync()
        {
            return _httpFactory.GetFromJsonAsync<VendorDashboardDto>("api/vendor/dashboard");
        }
    }
}
