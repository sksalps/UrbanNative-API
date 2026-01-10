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

            var url = "api/admin/returnsorder";
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
            return await _http.GetFromJsonAsync<AdminReturnDetailsDto>($"api/admin/returnsorder/{returnId}");

        }

        
        // ===============================
        // Approve / Reject
        // ===============================
        /*     public async Task<bool> ApproveRejectAsync(
                 int returnId,
                 bool isApproved,
                 string adminComment,
                 string remarkText)
             {
                 var response = await _http.PostAsJsonAsync(
                     $"api/admin/returnsorder/{returnId}/decision",
                     new
                     {
                         IsApproved = isApproved,
                         AdminComment = adminComment,
                         RemarkText = remarkText
                     });

                 return response.IsSuccessStatusCode;
             }

             */
        public async Task<bool> UpdateStatusAsync(
    int returnId,
    string newStatus,
    string adminComment,
    string remarkText)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/admin/returnsorder/{returnId}/status",
                new
                {
                    NewStatus = newStatus,
                    AdminComment = adminComment,
                    RemarkText = remarkText
                });

            return response.IsSuccessStatusCode;
        }

        // ===============================
        // Create Return Shipment
        // ===============================
     

        public async Task<bool> CreateReturnShipmentAsync(int returnId,int logisticsProviderID,
            string trackingNumber)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/admin/returnsorder/{returnId}/shipment",
                new
                {
                    LogisticsProviderID = logisticsProviderID,
                    TrackingNumber = trackingNumber
                });

            return response.IsSuccessStatusCode;
        }


        public async Task<List<ReturnImageDto>> GetImagesAsync(int returnId)
        {
            return await _http.GetFromJsonAsync<List<ReturnImageDto>>(
                $"api/admin/returnsorder/{returnId}/images") ?? new();
        }
        public async Task<IEnumerable<LogisticsProviderDto>> GetLogisticsProvidersAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<LogisticsProviderDto>>(
                "api/admin/returnsorder/logisticsproviders") ?? Enumerable.Empty<LogisticsProviderDto>();
        }



    }
}
