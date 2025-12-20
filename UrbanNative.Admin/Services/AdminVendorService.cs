using System.Text.Json;
using UrbanNative.Application.DTOs.AdminVendor;

namespace UrbanNative.Admin.Services
{
    public class AdminVendorService : IAdminVendorService
    {
        private readonly HttpClient _http;

        public AdminVendorService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // =========================
        // Admin – Vendors Listing
        // =========================
        public async Task<IEnumerable<AdminVendorListDto>> GetVendorsAsync(
            string? search,
            string? approvalStatus,
            bool? isActive)
        {
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(search))
                query.Add($"search={Uri.EscapeDataString(search)}");

            if (!string.IsNullOrWhiteSpace(approvalStatus))
                query.Add($"approvalStatus={approvalStatus}");

            if (isActive.HasValue)
                query.Add($"isActive={isActive.Value}");

            var url = "/api/admin/vendors";
            if (query.Any())
                url += "?" + string.Join("&", query);

            return await _http.GetFromJsonAsync<IEnumerable<AdminVendorListDto>>(url)
                   ?? Enumerable.Empty<AdminVendorListDto>();
        }

        // =========================
        // Admin – Single Vendor
        // =========================
        public async Task<AdminVendorDetailDto?> GetByIdAsync(int vendorId)
        {
            var res = await _http.GetAsync($"/api/admin/vendors/{vendorId}");

            if (!res.IsSuccessStatusCode)
                return null;

            return await res.Content.ReadFromJsonAsync<AdminVendorDetailDto>();
        }

        public async Task<VendorReferralInfoDto?> GetVendorReferralAsync(int vendorId)
        {
            var res = await _http.GetAsync($"/api/admin/vendors/{vendorId}/referral");

            if (!res.IsSuccessStatusCode)
                return null;

            if (res.Content.Headers.ContentLength == 0)
                return null;

            var json = await res.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<VendorReferralInfoDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }


        public async Task<IEnumerable<VendorMediaDto>> GetVendorMediaAsync(int vendorId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<VendorMediaDto>>(
                $"/api/admin/vendors/{vendorId}/media")
                ?? Enumerable.Empty<VendorMediaDto>();
        }

        // =========================
        // Admin – Approve / Reject Vendor
        // =========================
        public async Task UpdateApprovalAsync(
            int vendorId,
            string approvalStatus,
            string? reason)
        {
            var payload = new
            {
                VendorID = vendorId,
                ApprovalStatus = approvalStatus,
                Reason = reason
            };

            var res = await _http.PostAsJsonAsync(
                "/api/admin/vendors/approval",
                payload
            );

            res.EnsureSuccessStatusCode();
        }

        // =========================
        // Admin – Activate / Deactivate Vendor
        // =========================
        public async Task ToggleActiveAsync(int vendorId)
        {
            var res = await _http.PostAsJsonAsync(
                "/api/admin/vendors/activate",
                vendorId   // 🔥 IMPORTANT
            );

            res.EnsureSuccessStatusCode();
        }

    }
}
