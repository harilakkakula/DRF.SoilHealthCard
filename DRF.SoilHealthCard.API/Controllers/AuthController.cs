using DRF.SoilHealthCard.API.DTO;
using DRF.SoilHealthCard.API.Model;
using DRF.SoilHealthCard.API.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JwtAuthExample.Controllers
{
    /// <summary>
    /// AuthController
    /// </summary>
    /// <param name="userService"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(
                              IUserService userService
                             )
        {
            _userService = userService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest model)
        {
            if (model == null)
                return BadRequest("Invalid login data.");

           var result=_userService.Authenticate(model.Username,model.Password,"00");

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
           
            await _userService.RegisterAsync(registerDto);
            return Ok(new { Message = "Registration successful." });
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
