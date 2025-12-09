using Microsoft.Data.SqlClient;
using System.Data;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlConnectionFactory _factory;

        public ProductRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        // ============================================
        // 1️⃣ ADD PRODUCT
        // ============================================
        public async Task<int> AddProductAsync(Product p)
        {
            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_AddProduct", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@VendorID", p.VendorID);
            cmd.Parameters.AddWithValue("@CategoryID", p.CategoryID);
            cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
            cmd.Parameters.AddWithValue("@Description", p.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MRP", p.MRP);
            cmd.Parameters.AddWithValue("@DiscountPrice", p.DiscountPrice);
            cmd.Parameters.AddWithValue("@GSTPercentage", p.GSTPercentage);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        // ============================================
        // 2️⃣ UPDATE PRODUCT
        // ============================================
        public async Task<bool> UpdateProductAsync(Product p)
        {
            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_UpdateProduct", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ProductID", p.ProductID);
            cmd.Parameters.AddWithValue("@CategoryID", p.CategoryID);
            cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
            cmd.Parameters.AddWithValue("@Description", p.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MRP", p.MRP);
            cmd.Parameters.AddWithValue("@DiscountPrice", p.DiscountPrice);
            cmd.Parameters.AddWithValue("@GSTPercentage", p.GSTPercentage);
            cmd.Parameters.AddWithValue("@Stock", p.Stock);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        // ============================================
        // 3️⃣ APPROVE PRODUCT
        // ============================================
        public async Task<bool> ApproveProductAsync(int productId, int approvedBy)
        {
            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_ApproveProduct", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ProductID", productId);
            cmd.Parameters.AddWithValue("@ApprovedBy", approvedBy);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        // ============================================
        // 4️⃣ REJECT PRODUCT
        // ============================================
        public async Task<bool> RejectProductAsync(int productId, string reason)
        {
            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_RejectProduct", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ProductID", productId);
            cmd.Parameters.AddWithValue("@Reason", reason);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        // ============================================
        // 5️⃣ ADD PRODUCT IMAGE
        // ============================================
        public async Task<bool> AddProductImageAsync(int productId, string imageUrl, bool isPrimary)
        {
            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_AddProductImage", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ProductID", productId);
            cmd.Parameters.AddWithValue("@ImageURL", imageUrl);
            cmd.Parameters.AddWithValue("@IsPrimary", isPrimary);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        // ============================================
        // 6️⃣ GET PRODUCT BY ID
        // ============================================
        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_GetProductById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ProductID", productId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!reader.Read()) return null;

            return new Product
            {
                ProductID = productId,
                CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                ProductName = reader["ProductName"].ToString()!,
                MRP = reader.GetDecimal(reader.GetOrdinal("MRP")),
                DiscountPrice = reader.IsDBNull(reader.GetOrdinal("DiscountPrice")) ? 0 : reader.GetDecimal(reader.GetOrdinal("DiscountPrice")),
                Description = reader["Description"]?.ToString(),
                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                VendorID = reader.GetInt32(reader.GetOrdinal("VendorID")),
                GSTPercentage = reader.GetDecimal(reader.GetOrdinal("GSTPercentage")),
                CGST = reader["CGST"] as decimal?,
                SGST = reader["SGST"] as decimal?,
                IGST = reader["IGST"] as decimal?,
                ApprovalStatus = reader["ApprovalStatus"]?.ToString()
            };
        }

        // ============================================
        // 7️⃣ LIST PRODUCTS BY VENDOR
        // ============================================
        public async Task<IEnumerable<Product>> GetProductsByVendorAsync(int vendorId)
        {
            var products = new List<Product>();

            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_GetProductsByVendor", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@VendorID", vendorId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    ProductName = reader["ProductName"].ToString()!,
                    MRP = reader.GetDecimal(reader.GetOrdinal("MRP")),
                    DiscountPrice = reader.GetDecimal(reader.GetOrdinal("DiscountPrice")),
                    GSTPercentage = reader.GetDecimal(reader.GetOrdinal("GSTPercentage")),
                    VendorID = vendorId
                });
            }

            return products;
        }

        // ============================================
        // 8️⃣ LIST ALL APPROVED PRODUCTS
        // ============================================
        public async Task<IEnumerable<Product>> GetApprovedProductsAsync()
        {
            var products = new List<Product>();

            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_GetApprovedProducts", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    ProductName = reader["ProductName"].ToString()!,
                    MRP = reader.GetDecimal(reader.GetOrdinal("MRP")),
                    DiscountPrice = reader.GetDecimal(reader.GetOrdinal("DiscountPrice")),
                    GSTPercentage = reader.GetDecimal(reader.GetOrdinal("GSTPercentage"))
                });
            }

            return products;
        }

        // ============================================
        // 9️⃣ LIST ALL PENDING APPROVAL PRODUCTS
        // ============================================
        public async Task<IEnumerable<Product>> GetPendingProductsAsync()
        {
            var products = new List<Product>();

            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_GetPendingProducts", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    ProductName = reader["ProductName"].ToString()!,
                    MRP = reader.GetDecimal(reader.GetOrdinal("MRP")),
                    DiscountPrice = reader.GetDecimal(reader.GetOrdinal("DiscountPrice"))
                });
            }

            return products;
        }
    }
}
