using System.Net.Http.Json;
using UrbanNative.Application.DTOs.AdminOrders;

namespace UrbanNative.Admin.Services
{
    public class AdminOrderService : IAdminOrderService
    {
        private readonly HttpClient _httpClient;

        public AdminOrderService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<List<AdminOrderListDto>> GetOrdersAsync(
    DateTime? fromDate,
    DateTime? toDate,
    string orderStatus)
        {
            var query = new List<string>();

            if (fromDate.HasValue)
                query.Add($"fromDate={Uri.EscapeDataString(fromDate.Value.ToString("O"))}");

            if (toDate.HasValue)
                query.Add($"toDate={Uri.EscapeDataString(toDate.Value.ToString("O"))}");

            if (!string.IsNullOrWhiteSpace(orderStatus))
                query.Add($"orderStatus={Uri.EscapeDataString(orderStatus)}");

            var url = "api/admin/orders";

            if (query.Any())
                url += "?" + string.Join("&", query);

            return await _httpClient.GetFromJsonAsync<List<AdminOrderListDto>>(url);
        }


        public async Task<AdminOrderDetailsDto> GetOrderDetailsAsync(int orderId)
        {
            return await _httpClient.GetFromJsonAsync<AdminOrderDetailsDto>(
                $"api/admin/orders/{orderId}");
        }
    }
}

