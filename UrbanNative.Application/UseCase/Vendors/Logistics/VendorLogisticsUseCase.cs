using UrbanNative.Application.DTOs.Vendors.Logistics;
using UrbanNative.Application.GlobalCall;
using UrbanNative.Application.Interfaces.UseCase.Logistics;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Domain.Exceptions;

namespace UrbanNative.Application.UseCase.Vendors.Logistics
{
    public class VendorLogisticsUseCase : IVendorLogisticsUseCase
    {
        private readonly IVendorLogisticsRepository _repository;

        public VendorLogisticsUseCase(IVendorLogisticsRepository repository)
        {
            _repository = repository;
        }

        /* ============================================================
         * GRID-1 : VENDOR LOGISTICS ORDERS
         * ============================================================ */
        public async Task<IReadOnlyList<VendorLogisticsOrderDto>> GetOrdersAsync(
            int vendorId,
            VendorLogisticsFilterDto filter)
        {
            // Always run system auto transitions before any read
            //await _repository.RunAutoTransitionsAsync(vendorId);

            return await _repository.GetLogisticsOrdersAsync(
                vendorId,
                filter.ShowCompleted,
                filter.SearchText
            );
        }

        /* ============================================================
         * GRID-2 : ORDER ITEMS (DISPATCH SELECTION)
         * ============================================================ */
        public async Task<IReadOnlyList<VendorLogisticsItemDto>> GetOrderItemsAsync(
            int vendorId,
            int orderId)
        {
            return await _repository.GetOrderItemsAsync(
                vendorId,
                orderId
            );
        }
        /* ============================================================
         * Item WH : ORDER ITEMS (Warehouse for pickup the shipment)
         * ============================================================ */
        public async Task<IReadOnlyList<VendorItemWarehouseDto>> GetItemWarehouseAsync(
            int vendorId,
            int orderId)
        {
            return await _repository.GetItemsWarehouseAsync(
                vendorId,
                orderId
            );
        }

        /* ============================================================
         * GRID-3 : SHIPMENTS LIST
         * ============================================================ */
        public async Task<IReadOnlyList<VendorLogisticsShipmentDto>> GetShipmentsAsync(
            int vendorId,
            int? orderId,
            bool showCompleted)
        {
            // Auto transitions before showing shipment state
            //await _repository.RunAutoTransitionsAsync(vendorId);

            return await _repository.GetShipmentsAsync(
                vendorId,
                orderId,
                showCompleted
            );
        }

        /* ============================================================
         * CREATE SHIPMENT (DISPATCH FLOW)
         * ============================================================ */
        public async Task<int> CreateShipmentAsync(int vendorId,CreateVendorShipmentDto dto)
        {
            if (dto.OrderItemIds == null || !dto.OrderItemIds.Any())
                throw new DomainValidationException(
                    "ERR003",
                    "No items selected for dispatch");

            var statusOrder = ShipmentStatusOrder.GetOrder(dto.InitialShipmentStatus);
            if (statusOrder == 0)
                throw new DomainValidationException(
                    "ERR003",
                    "Invalid initial shipment status");

            // Tracking is mandatory from PICKED_UP onwards
            if (statusOrder >= ShipmentStatusOrder.GetOrder("PICKED_UP")
                && string.IsNullOrWhiteSpace(dto.TrackingNo))
            {
                throw new DomainValidationException(
                    "ERR003",
                    "Tracking number is required");
            }

            return await _repository.CreateShipmentAsync(
                vendorId: vendorId,
                orderId: dto.OrderID,
                orderItemIds: dto.OrderItemIds,
                logisticsProviderId: dto.LogisticsProviderID,
                pickupWarehouseId: dto.WarehouseID,
                initialStatus: dto.InitialShipmentStatus,
                trackingNo: dto.TrackingNo,
                createdBy: "VENDOR"
            );
        }

        /* ============================================================
         * UPDATE SHIPMENT STATUS (GRID-3)
         * ============================================================ */
        public async Task UpdateShipmentStatusAsync(int vendorId,UpdateShipmentStatusDto dto)
        {
             var shipment = await _repository.GetShipmentSnapshotAsync(dto.ShipmentID,dto.ShipmentType);

            if (shipment == null)
                throw new DomainValidationException("ERR003", "Invalid shipment");

            if (shipment.VendorID != vendorId)
                throw new DomainValidationException(
                    "ERR003","Unauthorized shipment access");

            if (shipment.ShipmentStatus == "DELIVERED"
                || shipment.ShipmentStatus == "RETURN_TO_ORIGIN")
                throw new DomainValidationException(
                    "ERR003", "Shipment already completed");

            var currentOrder = ShipmentStatusOrder.GetOrder(shipment.ShipmentStatus);
            var newOrder = ShipmentStatusOrder.GetOrder(dto.NewShipmentStatus);

            if (newOrder < currentOrder)
                throw new DomainValidationException("ERR003", "Status downgrade is not allowed");

            // Provider cannot be changed after PICKED_UP
            if (currentOrder >= ShipmentStatusOrder.GetOrder("PICKED_UP")
                && dto.LogisticsProviderID != shipment.LogisticsProviderID)
            {
                throw new DomainValidationException(
                    "ERR003", "Logistics provider cannot be changed");
            }

            // Tracking mandatory from PICKED_UP onwards
            if (newOrder >= ShipmentStatusOrder.GetOrder("PICKED_UP")
                && string.IsNullOrWhiteSpace(dto.TrackingNo))
            {
                throw new DomainValidationException(
                    "ERR003",  "Tracking number is required");
            }

            await _repository.UpdateShipmentStatusAsync(
                shipmentId: dto.ShipmentID,
                shipmentType: dto.ShipmentType,
                vendorId: vendorId,
                newStatus: dto.NewShipmentStatus,
                logisticsProviderId: dto.LogisticsProviderID,
                pickupWarehouseId: dto.WarehouseID,
                trackingNo: dto.TrackingNo,
                updatedBy: "VENDOR"
            );
        }

        /* ============================================================
         * SMART SEARCH AUTOCOMPLETE
         * ============================================================ */
        public async Task<IReadOnlyList<VendorLogisticsSuggestionDto>>
            GetFilterSuggestionsAsync(int vendorId, string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
                return Array.Empty<VendorLogisticsSuggestionDto>();

            return await _repository.GetFilterSuggestionsAsync(
                vendorId,
                term.Trim()
            );
        }
    }
}
