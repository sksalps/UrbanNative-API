using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.Vendors
{
    public class VendorProductRepository : IVendorProductRepository
    {
        
        private readonly SqlConnectionFactory _connectionFactory;

        public VendorProductRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<List<VendorProductListDto>> GetVendorProductsAsync(
            int vendorId,
            string? search,
            int? categoryId,
            int? hsnId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@VendorID", vendorId);
            param.Add("@Search", search);
            param.Add("@CategoryID", categoryId);
            param.Add("@HSNID", hsnId);

            var data = await conn.QueryAsync<VendorProductListDto>(
                "sp_VendorProducts_List",
                param,
                commandType: CommandType.StoredProcedure);

            return data.ToList();
        }
        
        public async Task<List<CategoryLookupDto>> GetVendorCategoriesAsync(int? vendorId, bool? isActive)
        {
            using var conn = _connectionFactory.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@VendorID", vendorId);
            param.Add("@isActive", isActive);

            var data = await conn.QueryAsync<CategoryLookupDto>(
                "sp_VendorCategories_Lookup",
                param,
                commandType: CommandType.StoredProcedure);

            return data.ToList();
        }

        public async Task<List<HsnLookupDto>> GetVendorHsnListAsync()
        {
            using var conn = _connectionFactory.CreateConnection();

            var data = await conn.QueryAsync<HsnLookupDto>(
                "sp_VendorHSN_Lookup",
                commandType: CommandType.StoredProcedure);

            return data.ToList();
        }

        public async Task<int> CreateProductAsync(int vendorId, VendorProductCreateDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@VendorID", vendorId);
            p.Add("@CategoryID", dto.CategoryID);
            p.Add("@ProductName", dto.ProductName);
            p.Add("@Description", dto.Description);
            p.Add("@MRP", dto.MRP);
            p.Add("@DiscountPrice", dto.DiscountPrice);
            p.Add("@HasVariants", dto.HasVariants);
            p.Add("@VendorWarehouseAddressID", dto.VendorWarehouseAddressID);
            p.Add("@NewProductID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync(
                "sp_VendorProduct_Insert",
                p,
                commandType: CommandType.StoredProcedure);

            
            var newId = p.Get<int>("@NewProductID");

            return newId;
        }

        public async Task<VendorProductEditDto> GetProductForEditAsync(int vendorId, int productId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@VendorID", vendorId);
            p.Add("@ProductID", productId);

            return await conn.QuerySingleAsync<VendorProductEditDto>(
                "sp_VendorProduct_GetForEdit",
                p,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateProductAsync(int vendorId, VendorProductUpdateDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@VendorID", vendorId);
            p.Add("@ProductID", dto.ProductID);

            p.Add("@ProductName", dto.ProductName);
            p.Add("@Description", dto.Description);
            p.Add("@MRP", dto.MRP);
            p.Add("@DiscountPrice", dto.DiscountPrice);

            p.Add("@CategoryID", dto.CategoryID);
            p.Add("@HasVariants", dto.HasVariants);
            p.Add("@VendorSharedMargin", dto.VendorSharedMargin);
            p.Add("@VendorWarehouseAddressID", dto.VendorWarehouseAddressID);

            await conn.ExecuteAsync(
                "sp_VendorProduct_Update",
                p,
                commandType: CommandType.StoredProcedure);
        }
        public async Task<List<VendorWarehouseDto>> GetVendorWarehousesAsync(
    int? vendorId,
    bool? isActive)
        {
            using var conn = _connectionFactory.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@VendorID", vendorId);
            p.Add("@IsActive", isActive);

            return (await conn.QueryAsync<VendorWarehouseDto>(
                "sp_VendorWarehouses_Lookup",
                p,
                commandType: CommandType.StoredProcedure)).ToList();
        }
        public async Task<List<ReturnPolicyDto>> GetReturnPoliciesAsync(
    bool? isActive)
        {
            using var conn = _connectionFactory.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@IsActive", isActive);

            return (await conn.QueryAsync<ReturnPolicyDto>(
                "sp_ReturnPolicy_Lookup",
                p,
                commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<HsnLookupDto> GetHsnByCategoryAsync(int categoryId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QuerySingleAsync<HsnLookupDto>(
                "sp_Category_HSN_Preview",
                new { CategoryID = categoryId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<VendorWarehouseDto> GetWarehouseByIdAsync(int addressId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QuerySingleAsync<VendorWarehouseDto>(
                "sp_VendorWarehouse_GetById",
                new { AddressID = addressId },
                commandType: CommandType.StoredProcedure); 

        }

    }
}
