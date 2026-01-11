using UrbanNative.Admin.Models;
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

        
        public async Task<AdminOrderPagedResultDto> GetOrdersAsync(OrderFilterModel filter)
        {
            var queryParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter.OrderNo))
                queryParts.Add($"orderNo={Uri.EscapeDataString(filter.OrderNo)}");

            if (filter.FromDate.HasValue)
                queryParts.Add($"fromDate={filter.FromDate.Value:O}");

            if (filter.ToDate.HasValue)
                queryParts.Add($"toDate={filter.ToDate.Value:O}");

            if (!string.IsNullOrWhiteSpace(filter.PaymentStatus))
                queryParts.Add($"paymentStatus={filter.PaymentStatus}");

            if (!string.IsNullOrWhiteSpace(filter.OrderStatus))
                queryParts.Add($"orderStatus={filter.OrderStatus}");

            if (filter.UserID.HasValue)
                queryParts.Add($"userId={filter.UserID.Value}");

            if (filter.VendorID.HasValue)
                queryParts.Add($"vendorId={filter.VendorID.Value}");

            queryParts.Add($"pageNumber={filter.PageNumber}");
            queryParts.Add($"pageSize={filter.PageSize}");

            var query = queryParts.Any()
                ? "?" + string.Join("&", queryParts)
                : string.Empty;

            var url = $"api/admin/orders{query}";

            return await _httpClient.GetFromJsonAsync<AdminOrderPagedResultDto>(url);
        }
        
        public async Task<AdminOrderShipmentDetailsDto> GetOrderShipmentDetailsAsync(int orderId)
        {
            return await _httpClient.GetFromJsonAsync<AdminOrderShipmentDetailsDto>($"api/admin/orders/{orderId}/shipments");
        }

        public async Task<AdminOrderDetailsDto> GetOrderDetailsAsync(int orderId)
        {
            return await _httpClient.GetFromJsonAsync<AdminOrderDetailsDto>(
                $"api/admin/orders/{orderId}/Orderdetails"           );
        }


        public async Task<byte[]> DownloadOrderPdfAsync(int orderId)
        {
            var response = await _httpClient.GetAsync(
                $"api/admin/orders/{orderId}/pdf"
            );

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

    }
}

