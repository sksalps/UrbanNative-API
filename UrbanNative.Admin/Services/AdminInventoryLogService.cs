using System;
using System.Net.Http.Json;
using UrbanNative.Application.DTOs.AdminInventory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UrbanNative.Admin.Services
{
    public class AdminInventoryLogService : IAdminInventoryLogService
    {
        private readonly HttpClient _http;

        public AdminInventoryLogService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }


        public async Task<AdminInventoryLogPeriodResultDto>    GetSkuLogsByPeriodAsync(int skuId, DateTime fromDate, DateTime toDate)
        {
            var url =
                $"api/admin/inventory/{skuId}/logs" +
                $"?fromDate={fromDate:yyyy-MM-dd}" +
                $"&toDate={toDate:yyyy-MM-dd}";

            return await _http.GetFromJsonAsync<AdminInventoryLogPeriodResultDto>(url)  ?? throw new Exception("Failed to fetch SKU inventory logs");
        }


    }

    

    }
