using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.AdminVendor;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    // This is used on Admin Dashboard for Vendor Management
    public class AdminVendorRepository : IAdminVendorRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminVendorRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // =========================
        // Admin – Vendors Listing
        // =========================
        public async Task<IEnumerable<AdminVendorListDto>> GetAdminVendorsAsync(
            string? search,
            string? approvalStatus,
            bool? isActive)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<AdminVendorListDto>(
                "sp_Admin_Vendors_GetAll",
                new
                {
                    Search = search,
                    ApprovalStatus = approvalStatus,
                    IsActive = isActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        // =========================
        // Admin – Vendor Details
        // =========================
        public async Task<AdminVendorDetailDto?> GetAdminVendorByIdAsync(int vendorId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<AdminVendorDetailDto>(
                "sp_Admin_Vendor_GetById",
                new { VendorID = vendorId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<VendorReferralInfoDto?> GetVendorReferralAsync(int vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<VendorReferralInfoDto>(
                "sp_Admin_GetVendorReferral",
                new { VendorID = vendorId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<VendorMediaDto>> GetVendorMediaAsync(int vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryAsync<VendorMediaDto>(
                "sp_Admin_GetVendorMedia",
                new { VendorID = vendorId },
                commandType: CommandType.StoredProcedure
            );
        }

        // =========================
        // Admin – Approve / Reject Vendor
        // (WITH HISTORY)
        // =========================
        public async Task<bool> UpdateVendorApprovalAsync(
            int vendorId,
            int adminId,
            string approvalStatus,
            string? reason)
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Admin_Vendor_UpdateApproval",
                new
                {
                    VendorID = vendorId,
                    AdminID = adminId,
                    ApprovalStatus = approvalStatus,
                    Reason = reason
                },
                commandType: CommandType.StoredProcedure
            );

            return true;
        }

        // =========================
        // Admin – Activate / Deactivate Vendor
        // =========================
        
        public async Task<bool> ToggleVendorActiveAsync(int vendorId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var rows = await connection.ExecuteAsync(
                "sp_Admin_Vendor_UpdateActive",
                new { VendorID = vendorId },
                commandType: CommandType.StoredProcedure
            );

            return rows > 0;
        }
    }
}
