using System.Net.Http.Json;

using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class VendorProductService: IVendorProductService
    {
        private readonly HttpClient _http;

        public VendorProductService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<List<VendorProductListDto>> GetMyProductsAsync()
        {
            var response = await _http.GetAsync("/api/vendors/products");
            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<List<VendorProductListDto>>()
                ?? new();
        }




    }

}
