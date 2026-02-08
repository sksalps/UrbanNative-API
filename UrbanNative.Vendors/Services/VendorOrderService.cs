using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using System.Text.Json;
using UrbanNative.Application.DTOs.Vendors.Orders;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class VendorOrderService : IVendorOrderService
    {
        private readonly HttpClient _http;

        public VendorOrderService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }
        public async Task<IReadOnlyList<VendorSkuProductSuggestionDto>>        GetSkuProductSuggestionsAsync(string term)
        {
            var response = await _http.GetAsync(
                $"api/vendors/orders/sku-product-suggestions?term={Uri.EscapeDataString(term)}"
            );

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<IReadOnlyList<VendorSkuProductSuggestionDto>>()
                ?? [];
        }
        public async Task<IReadOnlyList<VendorOrderListDto>> GetOrdersAsync(
            VendorOrderListFilterDto filter)
        {
            var response = await _http.PostAsJsonAsync(
                "api/vendors/orders/list", filter);

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<List<VendorOrderListDto>>()
                ?? new();
        }

        public async Task<VendorOrderSummaryDto> GetSummaryAsync(
            VendorOrderListFilterDto filter)
        {
            var response = await _http.PostAsJsonAsync(
                "api/vendors/orders/summary", filter);

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<VendorOrderSummaryDto>()
                ?? new VendorOrderSummaryDto();
        }

        // Vendor Order Details View
        public async Task<VendorOrderDetailsDto> GetOrderDetailsAsync(
    int? orderId,
    int? shipmentId,
    int? returnId)
        {
            var request = new VendorOrderDetailsRequestDto
            {
                OrderId = orderId,
                ShipmentId = shipmentId,
                ReturnId = returnId
            };

            var response = await _http.PostAsJsonAsync(
                "api/vendors/orders/details",
                request
            );

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Optional: deserialize API error contract
                throw new Exception(content);
            }

            return JsonSerializer.Deserialize<VendorOrderDetailsDto>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }


    }

}
