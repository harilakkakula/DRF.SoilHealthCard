using DRF.SoilHealthCard.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthExample.Controllers
{
    /// <summary>
    /// AuthController
    /// </summary>
    /// <param name="userService"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            var token = _userService.Authenticate(model.Username, model.Password);

            if (token == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            return Ok(new { Token = token });
        }
    }

    /// <summary>
    /// Login Request
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Username
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string? Password { get; set; }
    }
}
