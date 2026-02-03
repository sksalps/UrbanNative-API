using System.Net.Http;
using System.Text.Json;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Domain.Exceptions;
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
        // ======================================================
        // ADD INVENTORY TO SINGLE SKU
        // ======================================================
        public async Task<AddInventoryResultDto> AddSkuInventoryAsync(
            int skuId,
            int warehouseId,
            int quantity,
            string? remarks)
        {
            var request = new AddSkuInventoryRequestDto
            {
                SkuId = skuId,
                WarehouseId = warehouseId,
                Quantity = quantity,
                Remarks = remarks
            };

            var response = await _http.PostAsJsonAsync(
                "api/vendor/inventoryadd/sku/add",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new AddInventoryResultDto
                {
                    Success = false,
                    Message = error
                };
            }

            return await response.Content
                .ReadFromJsonAsync<AddInventoryResultDto>()
                ?? new AddInventoryResultDto
                {
                    Success = false,
                    Message = "Unexpected error while adding inventory."
                };
        }

        // ======================================================
        // Single SKU STOCK SUMMARY
        // ======================================================
        public async Task<SkuInventoryStockSummaryDto> GetSkuStockSummaryAsync(
    int skuId,
    int warehouseId)
        {
            try
            {
                var response = await _http.GetAsync(
                    $"api/vendor/inventoryadd/sku/{skuId}/stock?warehouseId={warehouseId}");

                if (!response.IsSuccessStatusCode)
                {
                    // Mask infra / SQL / SP issues
                    throw new DomainValidationException(
                        "Unable to load SKU summary. Please try again."
                    );
                }

                return await response.Content
                    .ReadFromJsonAsync<SkuInventoryStockSummaryDto>()
                    ?? new SkuInventoryStockSummaryDto();
            }
            catch (DomainValidationException)
            {
                // business-safe message → bubble
                throw;
            }
            catch (Exception)
            {
                // network / parsing / unexpected
                throw new DomainValidationException(
                    "Unable to load SKU summary. Please try again."
                );
            }
        }

        //==========SKU ADD END==========
        public async Task<InventoryInResponseDto>AddProductInventoryInAsync(ProductInventoryInRequestDto request)
        {
            var res = await _http
                .PostAsJsonAsync("api/vendor/inventoryadd/product/in", request);

            if (!res.IsSuccessStatusCode)
            {
                var errorJson = await res.Content.ReadAsStringAsync();

                var apiError = JsonSerializer.Deserialize<ApiErrorResponse>(
                    errorJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                throw new DomainValidationException(
                    apiError?.Message ?? "Inventory validation failed");
            }

            return await res.Content
                .ReadFromJsonAsync<InventoryInResponseDto>();
        }

        public async Task<IReadOnlyList<ProductAddInventorySkuGridDto>> GetProductSkusForInventoryAsync(int productId, int warehouseId)
        {
            var res = await _http.GetAsync($"/api/vendor/inventoryadd/product/{productId}/skus?warehouseId={warehouseId}");


            res.EnsureSuccessStatusCode();

            return await res.Content
                .ReadFromJsonAsync<IReadOnlyList<ProductAddInventorySkuGridDto>>();
        }


    }

}


