using DRF.SoilHealthCard.API.DBContext;
using DRF.SoilHealthCard.API.DTO;
using DRF.SoilHealthCard.API.Model;
using DRF.SoilHealthCard.API.Services.Interface;
using DRF.SoilHealthCard.API.Utils.Model;
using DRF.SoilHealthCard.API.Utils.Service;
using DRF.SoilHealthCard.API.ViewModel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using BCryptNet = BCrypt.Net.BCrypt;

namespace DRF.SoilHealthCard.API.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;
        private readonly IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;

        public UserService(AppDbContext context, IOptions<JwtSettings> jwtSettings, 
            IPasswordHasher<User> passwordHasher,
            ILogger<UserService> logger,
           IJwtUtils jwtUtils,
       IOptions<AppSettings> appSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;  // Accessing the SecretKey from appsettings.json
            _passwordHasher = passwordHasher;
            _logger = logger;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(string username, string password,string ipAddress)
        {
            var user = _context.Users.FirstOrDefault(x=>x.UserName== username);

            if (user == null || !BCryptNet.Verify(password, user.PasswordHash))
            {

            }

             string jwtToken = _jwtUtils.GenerateJwtToken(user);
             var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token, DefalutUrl());
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequestDto model)
        {
            User user = new();
            RegisterResponse response = new();

            // map model to new account object
            user.Email = model.Email;
            user.Name = "Admin";
            user.UserName = model.Email;
            user.Role = 1;
            user.RoleName = "Admin";
            user.EmailConfirmed = true;

            // hash password
            user.PasswordHash = BCryptNet.HashPassword(model.Password);
            user.IsActive = true;
            user.PhoneNumber = "";
            user.PhoneNumberConfirmed = true;
            user.NormalizedEmail = model.Email;
            user.EmailConfirmed = true;
            user.LastLoginDate = DateTime.Now;
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.AccessFailedCount = 0;
            user.NormalizedUserName = model.Email;
            user.LockoutEnd = DateTime.Now;
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            _context.Users.Add(user);
            _context.SaveChanges();
            return response;
            
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

        private string DefalutUrl()
        {
            string DefalutUrl = "/List";
            return DefalutUrl;
        }
    }
}
