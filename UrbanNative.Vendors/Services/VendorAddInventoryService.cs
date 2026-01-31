using System.Net.Http;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class VendorAddInventoryService : IVendorAddInventoryService
    {
        
        private readonly HttpClient _http;

        public VendorAddInventoryService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<InventoryInResponseDto> AddProductInventoryInAsync(ProductInventoryInRequestDto request)
        {
            var res = await _http.PostAsJsonAsync("api/vendor/inventoryadd/product/in", request);

            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            return await res.Content       .ReadFromJsonAsync<InventoryInResponseDto>();
        }


        public async Task<IReadOnlyList<ProductAddInventorySkuGridDto>> GetProductSkusForInventoryAsync(int productId,int warehouseId)
        {
            var res = await _http.GetAsync( $"/api/vendor/inventoryadd/product/{productId}/skus?warehouseId={warehouseId}");


            res.EnsureSuccessStatusCode();

            return await res.Content
                .ReadFromJsonAsync<IReadOnlyList<ProductAddInventorySkuGridDto>>();
        }


    }

}


