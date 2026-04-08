//using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Vendors.Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace UrbanNative.Vendors.Services
{
    public class VendorWarehouseService : IVendorWarehouseService
    {
        private readonly HttpClient _http;
        public VendorWarehouseService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }
        public async Task<List<VendorWarehouseListDto>> GetWarehousesAsync()
        {
            return await _http.GetFromJsonAsync<List<VendorWarehouseListDto>>("api/vendors/warehouses/list");
        }
        public async Task<VendorWarehouseSaveDto> GetWarehouseByIdAsync(int? warehouseId)
        {
            var res = await _http.GetAsync($"api/vendors/warehouses/{warehouseId}");
            res.EnsureSuccessStatusCode();

            return await res.Content.ReadFromJsonAsync<VendorWarehouseSaveDto>();
        }
        public async Task SaveWarehouseAsync(VendorWarehouseSaveDto dto)
        {
            await _http.PostAsJsonAsync("api/vendors/warehouses/savewh", dto);
        }
        public async Task<ServiceResult> DeleteWarehouseAsync(int warehouseId)
        {
            var res = await _http.PostAsJsonAsync("api/vendors/warehouses/deletewh", warehouseId);
            var content = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode)
            {
                return new ServiceResult { IsSuccess = false, Message = content };
            }

            return new ServiceResult { IsSuccess = true, Message = content };
        }
    }
}
