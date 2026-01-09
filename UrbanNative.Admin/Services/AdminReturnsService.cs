using System.Net.Http.Json;
using UrbanNative.Application.DTOs.AdminReturnsOrder;

namespace UrbanNative.Admin.Services
{
    
    public class AdminReturnsService : IAdminReturnsService
    {
        private readonly HttpClient _http;

        public AdminReturnsService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // ===============================
        // Returns Listing
        // ===============================
        public async Task<IEnumerable<AdminReturnListDto>> GetReturnsAsync(
            string? status,
            int? vendorId,
            DateTime? fromDate,
            DateTime? toDate)
        {
            var query = new List<string>();

            if (!string.IsNullOrEmpty(status))
                query.Add($"status={status}");

            if (vendorId.HasValue)
                query.Add($"vendorId={vendorId}");

            if (fromDate.HasValue)
                query.Add($"fromDate={fromDate:yyyy-MM-dd}");

            if (toDate.HasValue)
                query.Add($"toDate={toDate:yyyy-MM-dd}");

            var url = "api/admin/returns";
            if (query.Any())
                url += "?" + string.Join("&", query);

            return await _http.GetFromJsonAsync<IEnumerable<AdminReturnListDto>>(url)
                   ?? Enumerable.Empty<AdminReturnListDto>();
        }

        // ===============================
        // Return Details
        // ===============================
        public async Task<AdminReturnDetailsDto?> GetReturnDetailsAsync(int returnId)
        {
            return await _http.GetFromJsonAsync<AdminReturnDetailsDto>(
                $"api/admin/returns/{returnId}");
        }

        // ===============================
        // Approve / Reject
        // ===============================
        public async Task<bool> ApproveRejectAsync(
            int returnId,
            bool isApproved,
            string adminComment,
            string remarkText)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/admin/returns/{returnId}/decision",
                new
                {
                    IsApproved = isApproved,
                    AdminComment = adminComment,
                    RemarkText = remarkText
                });

            return response.IsSuccessStatusCode;
        }

        // ===============================
        // Create Return Shipment
        // ===============================
        public async Task<bool> CreateReturnShipmentAsync(
            int returnId,
            string courierName,
            string trackingNumber,
            string pickupAddress,
            string deliveryAddress)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/admin/returns/{returnId}/shipment",
                new
                {
                    CourierName = courierName,
                    TrackingNumber = trackingNumber,
                    PickupAddress = pickupAddress,
                    DeliveryAddress = deliveryAddress
                });

            return response.IsSuccessStatusCode;
        }
    }
}
