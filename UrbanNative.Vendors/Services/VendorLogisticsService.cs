using System.Net.Http.Json;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.DTOs.Vendors.Logistics;
using UrbanNative.Vendors.Services.Interfaces;

public class VendorLogisticsService : IVendorLogisticsService
{
    private readonly HttpClient _http;

    public VendorLogisticsService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiClient");
    }

    /* ================= GRID-1 ================= */

    public async Task<IReadOnlyList<VendorLogisticsOrderDto>> GetOrdersAsync(bool showCompleted,string? searchText = null)
    {
        var url = $"api/vendor/logistics/orders?showCompleted={showCompleted}";

        if (!string.IsNullOrWhiteSpace(searchText))
            url += $"&searchText={Uri.EscapeDataString(searchText)}";

        return await _http.GetFromJsonAsync<IReadOnlyList<VendorLogisticsOrderDto>>(url)
               ?? Array.Empty<VendorLogisticsOrderDto>();
    }

    /* ================= GRID-2 ================= */    

    public async Task<IReadOnlyList<VendorLogisticsItemDto>> GetOrderItemsAsync(int orderId)
    {
        return await _http.GetFromJsonAsync<IReadOnlyList<VendorLogisticsItemDto>>(
            $"api/vendor/logistics/items?orderId={orderId}"
        ) ?? Array.Empty<VendorLogisticsItemDto>();
    }
    
    //  Fetch Only Warehouse to pick the item of the order of this vendor
    public async Task<IReadOnlyList<VendorItemWarehouseDto>> GetItemlWarehousesAsync(int orderId)
        => await _http.GetFromJsonAsync<List<VendorItemWarehouseDto>>(
                $"api/vendor/logistics/warehouse?orderId={orderId}") ?? new();

    /* ================= GRID-3 ================= */

    public async Task<IReadOnlyList<VendorLogisticsShipmentDto>> GetShipmentsAsync(
        int? orderId,
        bool showCompleted)
    {
        var url = $"api/vendor/logistics/shipments?showCompleted={showCompleted}";

        if (orderId.HasValue)
            url += $"&orderId={orderId.Value}";

        return await _http.GetFromJsonAsync<IReadOnlyList<VendorLogisticsShipmentDto>>(url)
               ?? Array.Empty<VendorLogisticsShipmentDto>();
    }

    /* ================= CREATE SHIPMENT ================= */

    public async Task<int> CreateShipmentAsync(CreateVendorShipmentDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/vendor/logistics/shipments",  dto);

        res.EnsureSuccessStatusCode();

        var result = await res.Content.ReadFromJsonAsync<CreateShipmentResult>();

        return result!.ShipmentID;
    }

    /* ================= UPDATE SHIPMENT ================= */

    public async Task UpdateShipmentStatusAsync(UpdateShipmentStatusDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/vendor/logistics/shipments/update-status", dto);

        res.EnsureSuccessStatusCode();
    }

    /* ================= AUTOCOMPLETE ================= */

    public async Task<IReadOnlyList<VendorLogisticsSuggestionDto>> GetFilterSuggestionsAsync(string term)
    {
        return await _http.GetFromJsonAsync<IReadOnlyList<VendorLogisticsSuggestionDto>>(
            $"api/vendor/logistics/suggestions?term={Uri.EscapeDataString(term)}"
        ) ?? Array.Empty<VendorLogisticsSuggestionDto>();
    }
    
    /* ================= PRIVATE RESPONSE DTO ================= */

    private sealed class CreateShipmentResult
    {
        public int ShipmentID { get; set; }
    }
}
