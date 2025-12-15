using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UrbanNative.Application.DTOs;
using UrbanNative.Application.Interfaces;


namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AdminController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        // -----------------------------------------------------
        // 1️⃣ LOGIN VALIDATION
        // POST: /api/admin/validate
        // -----------------------------------------------------
    
        // -----------------------------------------------------
        // 2️⃣ STATISTICS
        // Routes under: /api/admin/stats/*
        // -----------------------------------------------------

        [HttpGet("stats/vendors-count")]
        public async Task<ActionResult<int>> GetVendorsCount()
        {
            return await ExecuteCountQuery("SELECT COUNT(*) FROM Vendors");
        }

        [HttpGet("stats/pending-products")]
        public async Task<ActionResult<int>> GetPendingProducts()
        {
            return await ExecuteCountQuery("SELECT COUNT(*) FROM Products WHERE ApprovalStatus = 'Pending'");
        }

        [HttpGet("stats/low-stock-count")]
        public async Task<ActionResult<int>> GetLowStockCount()
        {
            return await ExecuteCountQuery("SELECT COUNT(*) FROM Products WHERE Stock < 10");
        }

        [HttpGet("stats/users-count")]
        public async Task<ActionResult<int>> GetUsersCount()
        {
            return await ExecuteCountQuery("SELECT COUNT(*) FROM Users");
        }

        // -----------------------------------------------------
        // Helper: Executes COUNT(*) queries
        // -----------------------------------------------------
        private async Task<int> ExecuteCountQuery(string sql)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand(sql, con);

            await con.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }

    // Request DTO for login
    public class AdminLoginRequest
    {
        public string Identifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
