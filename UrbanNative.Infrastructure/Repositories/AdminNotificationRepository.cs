using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.DTOs;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminNotificationRepository : IAdminNotificationRepository
    {
        private readonly string _conn;

        public AdminNotificationRepository(IConfiguration config)
        {
            var cs = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(cs))
                throw new ArgumentNullException(nameof(config),
                    "Connection string 'DefaultConnection' not found in configuration.");

            _conn = cs;
        }

        private SqlConnection GetCon() => new SqlConnection(_conn);

        public async Task<IEnumerable<AdminNotificationDto>> GetUnreadAsync(int adminId)
        {
            var list = new List<AdminNotificationDto>();

            using var con = GetCon();
            using var cmd = new SqlCommand(@"
                SELECT Id, AdminId, Title, Message, IsRead, CreatedAt
                FROM AdminNotifications
                WHERE AdminId = @AdminId AND IsRead = 0
                ORDER BY CreatedAt DESC
            ", con);

            cmd.Parameters.AddWithValue("@AdminId", adminId);

            await con.OpenAsync();
            using var rs = await cmd.ExecuteReaderAsync();

            while (await rs.ReadAsync())
            {
                list.Add(new AdminNotificationDto
                {
                    Id = rs.GetInt32(0),
                    AdminId = rs.GetInt32(1),
                    Title = rs.GetString(2),
                    Message = rs.IsDBNull(3) ? null : rs.GetString(3),
                    IsRead = rs.GetBoolean(4),
                    CreatedAt = rs.GetDateTime(5)
                });
            }

            return list;
        }

        public async Task<int> GetUnreadCountAsync(int adminId)
        {
            using var con = GetCon();
            using var cmd = new SqlCommand(@"
                SELECT COUNT(*) 
                FROM AdminNotifications 
                WHERE AdminId = @AdminId AND IsRead = 0
            ", con);

            cmd.Parameters.AddWithValue("@AdminId", adminId);

            await con.OpenAsync();
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task MarkAsReadAsync(int id)
        {
            using var con = GetCon();
            using var cmd = new SqlCommand(@"
                UPDATE AdminNotifications 
                SET IsRead = 1 
                WHERE Id = @Id
            ", con);

            cmd.Parameters.AddWithValue("@Id", id);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task CreateAsync(AdminNotificationDto notification)
        {
            using var con = GetCon();
            using var cmd = new SqlCommand(@"
                INSERT INTO AdminNotifications (AdminId, Title, Message)
                VALUES (@AdminId, @Title, @Message)
            ", con);

            cmd.Parameters.AddWithValue("@AdminId", notification.AdminId);
            cmd.Parameters.AddWithValue("@Title", notification.Title);
            cmd.Parameters.AddWithValue("@Message", (object?)notification.Message ?? DBNull.Value);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
