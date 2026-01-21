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
        public async Task<List<VendorProductListDto>> GetVendorProductsAsync(int vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@VendorID", vendorId);

            var data = await conn.QueryAsync<VendorProductListDto>(
                "sp_VendorProducts_List",
                param,
                commandType: CommandType.StoredProcedure);

            return data.ToList();
        }

    }
}
