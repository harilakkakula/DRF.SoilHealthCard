using DRF.SoilHealthCard.API.DBContext;
using DRF.SoilHealthCard.API.DTO;
using DRF.SoilHealthCard.API.Model;
using DRF.SoilHealthCard.API.Services.Interface;
using DRF.SoilHealthCard.API.ViewModel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DRF.SoilHealthCard.API.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(AppDbContext context, IOptions<JwtSettings> jwtSettings, 
            IPasswordHasher<User> passwordHasher,
            ILogger<UserService> logger)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;  // Accessing the SecretKey from appsettings.json
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public string Authenticate(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.UserName == username);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null; // Authentication failed
            }

            // Create JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);  // Using the SecretKey from appsettings.json
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.RoleName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto model)
        {
            if (await UserExistsAsync(model.Username))
                return new RegisterResponseDto { Success = false, Message = "User already exists" };

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RegisterResponseDto { Success = true, Message = "User registered successfully" };
        }

        private async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var salt = Convert.FromBase64String(storedHash.Substring(0, 24));
            var storedPasswordHash = storedHash.Substring(24);
            var hash = HashPassword(password, salt);
            return storedPasswordHash == hash;
        }

        private string HashPassword(string password, byte[] salt)
        {
            byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 10000, 256 / 8);
            return Convert.ToBase64String(hash);
        }
    }
}
