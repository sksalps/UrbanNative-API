using UrbanNative.Application.DTOs.Vendors.Logistics;

public interface IVendorLogisticsService
{
    /* Grid-1 */
    Task<IReadOnlyList<VendorLogisticsOrderDto>> GetOrdersAsync(bool showCompleted, string? searchText = null);

    /* Grid-2 */
    Task<IReadOnlyList<VendorLogisticsItemDto>> GetOrderItemsAsync(int orderId);


    /* Grid-3 */
    Task<IReadOnlyList<VendorLogisticsShipmentDto>> GetShipmentsAsync(
        int? orderId,
        bool showCompleted);

    /* Create Shipment */
    Task<int> CreateShipmentAsync(CreateVendorShipmentDto dto);

    /* Update Shipment Status */
    Task UpdateShipmentStatusAsync(UpdateShipmentStatusDto dto);

    /* Autocomplete / Smart Search */
    Task<IReadOnlyList<VendorLogisticsSuggestionDto>> GetFilterSuggestionsAsync(string term);
    //Resolve Text box autocomplete
    
}
