using UrbanNative.Application.DTOs.Vendors.Logistics;

namespace UrbanNative.Application.Interfaces.Vendors
{
    public interface IVendorLogisticsRepository
    {
        /* Grid-1 */
        Task<IReadOnlyList<VendorLogisticsOrderDto>> GetLogisticsOrdersAsync(
            int vendorId,
            bool showCompleted,
            string? searchText);

        /* Grid-2 */
        Task<IReadOnlyList<VendorLogisticsItemDto>> GetOrderItemsAsync(
            int vendorId,
            int orderId);
        /* Item Warehouse */ 
        Task<IReadOnlyList<VendorItemWarehouseDto>> GetItemsWarehouseAsync(
                int vendorId,
                int orderId);
        /* Grid-3 */
        Task<IReadOnlyList<VendorLogisticsShipmentDto>> GetShipmentsAsync(
            int vendorId,
            int? orderId,
            bool showCompleted);

        /* Create Shipment */
        Task<int> CreateShipmentAsync(
            int vendorId,
            int orderId,
            IReadOnlyList<int> orderItemIds,
            int logisticsProviderId,
            int pickupWarehouseId,
            string initialStatus,
            string? trackingNo,
            string createdBy);

        /* Update Shipment */
        Task UpdateShipmentStatusAsync(
            int shipmentId,
            string shipmentType,
            int vendorId,
            string newStatus,
            int? logisticsProviderId,
            int? pickupWarehouseId,
            string? trackingNo,
            string updatedBy);

        /* Snapshot (validation) */
        Task<VendorShipmentSnapshotDto?> GetShipmentSnapshotAsync(
            int shipmentId, string shipmentType);

        /* Auto transitions */
        Task RunAutoTransitionsAsync(int vendorId);

        /* Autocomplete */
        Task<IReadOnlyList<VendorLogisticsSuggestionDto>> GetFilterSuggestionsAsync(
            int vendorId,
            string term);
    }

}
