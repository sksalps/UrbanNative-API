using System.Data;
using Dapper;
using UrbanNative.Application.DTOs.Vendors.Logistics;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.Vendors
{
    public class VendorLogisticsRepository : IVendorLogisticsRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public VendorLogisticsRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /* ============================================================
         * GRID-1 : VENDOR LOGISTICS ORDERS
         * ============================================================ */
        public async Task<IReadOnlyList<VendorLogisticsOrderDto>>
            GetLogisticsOrdersAsync(
                int vendorId,
                bool showCompleted,
                string? searchText)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryAsync<VendorLogisticsOrderDto>(
                "sp_VendorLogisticsOrders_Grid1",
                new
                {
                    VendorID = vendorId,
                    ShowCompleted = showCompleted
                    //SearchText = searchText
                },
                commandType: CommandType.StoredProcedure);

            return result.AsList();
        }

        /* ============================================================
         * GRID-2 : ORDER ITEMS (DISPATCH SELECTION)
         * ============================================================ */
        public async Task<IReadOnlyList<VendorLogisticsItemDto>>
            GetOrderItemsAsync(
                int vendorId,
                int orderId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryAsync<VendorLogisticsItemDto>(
                "sp_VendorLogisticsItem_Grid2",
                new
                {
                    VendorID = vendorId,
                    OrderID = orderId
                },
                commandType: CommandType.StoredProcedure);

            return result.AsList();
        }

        /* ============================================================
         * GRID-3 : SHIPMENTS LIST
         * ============================================================ */
        public async Task<IReadOnlyList<VendorLogisticsShipmentDto>>
            GetShipmentsAsync(
                int vendorId,
                int? orderId,
                bool showCompleted)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryAsync<VendorLogisticsShipmentDto>(
                "sp_VendorLogisticsShipment_Grid3",
                new
                {
                    VendorID = vendorId,
                    OrderID = orderId,
                    ShowCompleted = showCompleted
                },
                commandType: CommandType.StoredProcedure);

            return result.AsList();
        }

        /* ============================================================
         * CREATE SHIPMENT
         * ============================================================ */
        public async Task<int> CreateShipmentAsync(
            int vendorId,
            int orderId,
            IReadOnlyList<int> orderItemIds,
            int logisticsProviderId,
            string initialStatus,
            string? trackingNo,
            string createdBy)
        {
            using var conn = _connectionFactory.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@VendorID", vendorId);
            p.Add("@OrderID", orderId);
            p.Add("@LogisticsProviderID", logisticsProviderId);
            p.Add("@InitialShipmentStatus", initialStatus);
            p.Add("@TrackingNo", trackingNo);
            p.Add("@CreatedBy", createdBy);

            // Table-valued parameter for items
            var tvp = new DataTable();
            tvp.Columns.Add("OrderItemID", typeof(int));
            foreach (var id in orderItemIds)
                tvp.Rows.Add(id);
            //p.Add("@OrderItemIds", tvp.AsTableValuedParameter("dbo.OrderItemID"));
            p.Add("@OrderItemIds", tvp.AsTableValuedParameter("dbo.OrderItemIdList"));

            p.Add("@ShipmentID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync(
                "sp_VendorLogisticsShipment_Create",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<int>("@ShipmentID");
        }

        /* ============================================================
         * UPDATE SHIPMENT STATUS
         * ============================================================ */
        public async Task UpdateShipmentStatusAsync(
            int shipmentId,
            int vendorId,
            string newStatus,
            int? logisticsProviderId,
            string? trackingNo,
            string updatedBy)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_VendorOrderShipment_UpdateStatus",
                new
                {
                    ShipmentID = shipmentId,
                    VendorID = vendorId,
                    NewStatus = newStatus,
                    LogisticsProviderID = logisticsProviderId,
                    TrackingNo = trackingNo,
                    UpdatedBy = updatedBy
                },
                commandType: CommandType.StoredProcedure);
        }

        /* ============================================================
         * SNAPSHOT (VALIDATION)
         * ============================================================ */
        public async Task<VendorShipmentSnapshotDto?>
            GetShipmentSnapshotAsync(int shipmentId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<VendorShipmentSnapshotDto>(
                "sp_VendorLogisticsShipment_Snapshot",
                new { ShipmentID = shipmentId },
                commandType: CommandType.StoredProcedure);
        }

        /* ============================================================
         * AUTO TRANSITIONS
         * ============================================================ */
        public async Task RunAutoTransitionsAsync(int vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_OrderShipment_AutoTransitions",
                new { VendorID = vendorId },
                commandType: CommandType.StoredProcedure);
        }

        /* ============================================================
         * SMART SEARCH AUTOCOMPLETE
         * ============================================================ */
        public async Task<IReadOnlyList<VendorLogisticsSuggestionDto>>
            GetFilterSuggestionsAsync(
                int vendorId,
                string term)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryAsync<VendorLogisticsSuggestionDto>(
                "sp_VendorLogistics_FilterSuggestions",
                new
                {
                    VendorID = vendorId,
                    Term = term
                },
                commandType: CommandType.StoredProcedure);

            return result.AsList();
        }
    }
}
