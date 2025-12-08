using Microsoft.AspNetCore.Mvc;
using UrbanNative.Api.Services;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public UserController(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var msg = await _messageService.GetMessage("USER_REGISTER_SUCCESS");

            var userId = await _userService.RegisterAsync(user);

            return Ok(new { Success = true, Message = msg, UserID = userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.LoginAsync(request.Mobile);

            if (user == null)
            {
                var errorMsg = await _messageService.GetMessage("INVALID_MOBILE");
                return Unauthorized(new { Message = errorMsg });
            }

            var token = _userService.GenerateJwtToken(user);
            var successMsg = await _messageService.GetMessage("USER_LOGIN_SUCCESS");

            return Ok(new { Success = true, Message = successMsg, Token = token });
        }

    }

    // DTO for login input
    public class LoginRequest
    {
        public string Mobile { get; set; } = string.Empty;
    }
}
