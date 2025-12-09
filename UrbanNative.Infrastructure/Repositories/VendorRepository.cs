
using Microsoft.Data.SqlClient;
using System.Data;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public VendorRepository(SqlConnectionFactory factory)
        {
            _connectionFactory = factory;
        }

        public async Task<int> RegisterVendorAsync(Vendor v)
        {
            using var conn = (SqlConnection)_connectionFactory.CreateConnection();
            using var cmd = new SqlCommand("sp_RegisterVendor", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VendorName", v.VendorName);
            cmd.Parameters.AddWithValue("@ContactPerson", v.ContactPerson ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Mobile", v.Mobile);
            cmd.Parameters.AddWithValue("@Email", v.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BusinessName", v.BusinessName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GSTNumber", v.GSTNumber ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PANNumber", v.PANNumber ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BusinessAddress", v.BusinessAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PickupAddress", v.PickupAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@State", v.State ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@StateCode", v.StateCode ?? (object)DBNull.Value);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<Vendor?> LoginVendorAsync(string mobile)
        {
            using var conn = (SqlConnection)_connectionFactory.CreateConnection();
            using var cmd = new SqlCommand("sp_LoginVendor", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mobile", mobile);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.Read()) return null;

            return new Vendor
            {
                VendorID = reader.GetInt32(reader.GetOrdinal("VendorID")),
                VendorName = reader["VendorName"].ToString()!,
                ContactPerson = reader["ContactPerson"]?.ToString(),
                Mobile = mobile,
                Email = reader["Email"]?.ToString(),
                BusinessName = reader["BusinessName"]?.ToString(),
                GSTNumber = reader["GSTNumber"]?.ToString(),
                ApprovalStatus = reader["ApprovalStatus"].ToString()!
            };
        }

        public async Task<bool> ApproveVendorAsync(int vendorId, int approvedBy)
        {
            using var conn = (SqlConnection)_connectionFactory.CreateConnection();
            using var cmd = new SqlCommand("sp_ApproveVendor", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VendorID", vendorId);
            cmd.Parameters.AddWithValue("@ApprovedBy", approvedBy);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<bool> RejectVendorAsync(int vendorId, string reason, int rejectedBy)
        {
            using var conn = (SqlConnection)_connectionFactory.CreateConnection();
            using var cmd = new SqlCommand("sp_RejectVendor", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VendorID", vendorId);
            cmd.Parameters.AddWithValue("@RejectedBy", rejectedBy);
            cmd.Parameters.AddWithValue("@RejectionReason", reason);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<bool> UpdateVendorAsync(Vendor vendor)
        {
            using var conn = (SqlConnection)_connectionFactory.CreateConnection();
            using var cmd = new SqlCommand("sp_UpdateVendor", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VendorID", vendor.VendorID);
            cmd.Parameters.AddWithValue("@VendorName", vendor.VendorName);
            cmd.Parameters.AddWithValue("@ContactPerson", vendor.ContactPerson ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", vendor.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BusinessName", vendor.BusinessName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GSTNumber", vendor.GSTNumber ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PANNumber", vendor.PANNumber ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BusinessAddress", vendor.BusinessAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PickupAddress", vendor.PickupAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@State", vendor.State ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@StateCode", vendor.StateCode ?? (object)DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<Vendor?> GetVendorByIdAsync(int vendorId)
        {
            using var conn = (SqlConnection)_connectionFactory.CreateConnection();
            using var cmd = new SqlCommand("sp_GetVendorDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VendorID", vendorId);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.Read()) return null;

            return new Vendor
            {
                VendorID = vendorId,
                VendorName = reader["VendorName"].ToString()!,
                ContactPerson = reader["ContactPerson"]?.ToString(),
                Mobile = reader["Mobile"].ToString()!,
                Email = reader["Email"]?.ToString(),
                ApprovalStatus = reader["ApprovalStatus"].ToString()!
            };
        }
    }
}