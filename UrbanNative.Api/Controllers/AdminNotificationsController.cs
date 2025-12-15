using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.DTOs;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin/notifications")]
    [Authorize(Roles = "Admin")]
    public class AdminNotificationsController : ControllerBase
    {
        private readonly IAdminNotificationRepository _repo;

        public AdminNotificationsController(IAdminNotificationRepository repo)
        {
            _repo = repo;
        }

        private int GetAdminId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        // 🔔 GET unread notifications (current admin)
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnread()
        {
            var adminId = GetAdminId();
            var list = await _repo.GetUnreadAsync(adminId);
            return Ok(list);
        }

        // 🔢 GET unread count
        [HttpGet("count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var adminId = GetAdminId();
            var count = await _repo.GetUnreadCountAsync(adminId);
            return Ok(count);
        }

        // ✅ Mark notification as read
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            await _repo.MarkAsReadAsync(id);
            return Ok();
        }
    }
}
