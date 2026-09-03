using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.UseCases.Customers;


namespace UrbanNative.Api.Controllers.Customers
{
    [ApiController]
    [Route("api/customer/dashboard")]
    [Authorize] // 🔐 Secure endpoint
    public class CustomerDashboardController : ControllerBase
    {
        private readonly ICustomerDashboardUseCase _dashboardUseCase;

        public CustomerDashboardController(ICustomerDashboardUseCase dashboardUseCase)
        {
            _dashboardUseCase = dashboardUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                // 🔷 Extract UserID from JWT
                var userIdClaim = User.FindFirst("UserID")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("Invalid token");

                int userId = Convert.ToInt32(userIdClaim);

                // 🔷 Call UseCase
                var result = await _dashboardUseCase.GetDashboardAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // 🔷 Log error (future improvement)
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
