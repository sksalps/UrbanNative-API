using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.Common;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.CommonCrossDashboard
{
    public class SkuFilterRepository : ISkuFilterRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;
        public SkuFilterRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // =====================================================
        // Categories
        // =====================================================
        public async Task<IReadOnlyList<SkuCategoryDto>> GetCategoriesAsync(
            int? vendorId,
            bool? isActive)
        {
            using var conn = _connectionFactory.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@VendorID", vendorId);
            param.Add("@IsActive", isActive);

            var data = await conn.QueryAsync<SkuCategoryDto>(
                "sp_SkuFilter_Categories",
                param,
                commandType: CommandType.StoredProcedure);

            return data.AsList();
        }

        // =====================================================
        // Products
        // =====================================================
        public async Task<IReadOnlyList<SkuProductDto>> GetProductsAsync(
            int? vendorId,
            int categoryId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@VendorID", vendorId);
            param.Add("@CategoryID", categoryId);

            var data = await conn.QueryAsync<SkuProductDto>(
                "sp_SkuFilter_Products",
                param,
                commandType: CommandType.StoredProcedure);

            return data.AsList();
        }

        // =====================================================
        // SKU Typeahead
        // =====================================================
        public async Task<IReadOnlyList<SkuLookupDto>> SearchSkusAsync(
            int? vendorId,
            int productId,
            string? search)
        {
            using var conn = _connectionFactory.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@VendorID", vendorId);
            param.Add("@ProductID", productId);
            param.Add("@Search", search);

            var data = await conn.QueryAsync<SkuLookupDto>(
                "sp_SkuFilter_SKUs",
                param,
                commandType: CommandType.StoredProcedure);

            return data.AsList();
        }

        // =====================================================
        // SKU Context (Category + Product resolution)
        // =====================================================
        public async Task<SkuContextDto?> GetSkuContextAsync(
            int skuId,
            int? vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@SKUId", skuId);
            param.Add("@VendorID", vendorId);

            return await conn.QueryFirstOrDefaultAsync<SkuContextDto>(
                "sp_SkuFilter_SkuContext",
                param,
                commandType: CommandType.StoredProcedure);
        }


        public async Task<HsnLookupDto> GetHsnByCategoryAsync(int categoryId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QuerySingleAsync<HsnLookupDto>(
                "sp_Category_HSN_Preview",
                new { CategoryID = categoryId },
                commandType: CommandType.StoredProcedure);
        }

        // =====================================================
        // Warehouse Lookup for common use case
        // =====================================================

        //Fetch a Vendor Warehouse by AddressID for preview
        public async Task<WarehousePreviewDto> GetWarehouseByIdAsync(int addressId)
        {
            using var conn = _connectionFactory.CreateConnection();
            var p = new DynamicParameters();
            p.Add("@AddressID", addressId);

            var data= (await conn.QueryFirstOrDefaultAsync<WarehousePreviewDto>(
                "sp_VendorWarehouse_GetById",
                p,
                commandType: CommandType.StoredProcedure));           
            return data;
        }
        //Fetch Vendor Warehouses or all active/inactive warehouses
        public async Task<IReadOnlyList<CommonWarehouseDto>> GetWarehousesAsync(int? vendorId,bool? isActive)
        {
            using var conn = _connectionFactory.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@VendorID", vendorId);
            p.Add("@IsActive", isActive);

            return (await conn.QueryAsync<CommonWarehouseDto>(
                "sp_VendorWarehouses_Lookup",
                p,
                commandType: CommandType.StoredProcedure)).ToList();
        }
    }
}




