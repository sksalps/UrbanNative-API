using UrbanNative.Application.DTOs.Vendors.Logistics;

namespace UrbanNative.Application.Interfaces.UseCase.Logistics
{
    public interface IVendorLogisticsUseCase
    {
        /* Grid-1 */
        Task<IReadOnlyList<VendorLogisticsOrderDto>> GetOrdersAsync(
            int vendorId,
            VendorLogisticsFilterDto filter);

        /* Grid-2 */       
        Task<IReadOnlyList<VendorLogisticsItemDto>> GetOrderItemsAsync(
            int vendorId,
            int orderId);
        /* Item Warehouse */
        Task<IReadOnlyList<VendorItemWarehouseDto>> GetItemWarehouseAsync(
            int vendorId,
            int orderId);
        /* Order Summary */
        Task<VendorOrderSummaryDto?> GetOrderSummaryAsync(
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
            CreateVendorShipmentDto dto);

        /* Update Shipment */
        Task UpdateShipmentStatusAsync(
            int vendorId,
            UpdateShipmentStatusDto dto);

        /* Autocomplete */
        Task<IReadOnlyList<VendorLogisticsSuggestionDto>> GetFilterSuggestionsAsync(
            int vendorId,
            string term);
    }

}
