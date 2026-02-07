using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Orders;
using UrbanNative.Application.Interfaces.Vendors.Orders;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.Vendors.Orders
{
    public class VendorOrderRepository : IVendorOrderRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public VendorOrderRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<VendorOrderListDto>> GetVendorOrdersAsync(
            VendorOrderListFilterDto filter)
        {
            using var conn = _connectionFactory.CreateConnection();
            var result = await conn.QueryAsync<VendorOrderListDto>(
                "sp_VendorOrders_List",
                new
                {
                    filter.VendorID,
                    filter.FromDate,
                    filter.ToDate,
                    filter.OrderNo,
                    filter.PaymentStatus,
                    filter.ShipmentStatus,
                    filter.SearchType,
                    filter.SkuOrProduct,
                    filter.RTOFilter,
                    filter.PageNumber,
                    filter.PageSize
                },
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<VendorOrderSummaryDto> GetVendorOrderSummaryAsync(
            VendorOrderListFilterDto filter)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryFirstAsync<VendorOrderSummaryDto>(
                "sp_VendorOrders_Summary",
                new
                {
                    filter.VendorID,
                    filter.FromDate,
                    filter.ToDate,
                    filter.OrderNo,
                    filter.SkuOrProduct
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IReadOnlyList<VendorSkuProductSuggestionDto>>
    GetSkuProductSuggestionsAsync(int vendorId, string term)
        {
            using var conn = _connectionFactory.CreateConnection();
            var result= (await conn.QueryAsync<VendorSkuProductSuggestionDto>(
                "sp_VendorSkuProduct_Autocomplete_OrderedOnly",
                new { VendorID = vendorId, Term = term },
                commandType: CommandType.StoredProcedure
            )).ToList();
            return result;
        }

    }

}
