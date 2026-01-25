using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class VendorSkuService: IVendorSkuService
    {
        private readonly HttpClient _http;

        public VendorSkuService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<VendorSkuHeaderDto> GetHeaderAsync(int productId)
        {
            return await _http.GetFromJsonAsync<VendorSkuHeaderDto>(
                $"api/vendors/productSKU/{productId}/sku-header"
            ) ?? new VendorSkuHeaderDto();
        }

        public async Task<List<VendorSkuGridDto>> GetGridAsync(int productId)
        {
            return await _http.GetFromJsonAsync<List<VendorSkuGridDto>>(
                $"api/vendors/productSKU/{productId}/skus"
            ) ?? new List<VendorSkuGridDto>();
        }

        public async Task SaveAsync(int productId, List<VendorSkuSaveDto> skus)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/vendors/productSKU/{productId}/skus",
                skus
            );

            response.EnsureSuccessStatusCode();
        }
    }
}
