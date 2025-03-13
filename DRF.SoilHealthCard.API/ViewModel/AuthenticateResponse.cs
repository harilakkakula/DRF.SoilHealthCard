using DRF.SoilHealthCard.API.Model;
using System.Text.Json.Serialization;

namespace DRF.SoilHealthCard.API.ViewModel
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public DateTime LastLogindate { get; set; }
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public string DefalutUrl { get; set; }
        public string Role { get; set; }
        public string Mobile { get; set; }
        public AuthenticateResponse(User user, string jwtToken, string refreshToken,string _DefalutUrl)
        {
            Id = user.Id;
            Name = user.Name;
            Username = user.UserName;
            JwtToken = jwtToken;
            LastLogindate = user.LastLoginDate;
            RefreshToken = refreshToken;
            Role = user.RoleName;
            DefalutUrl = _DefalutUrl;
            Mobile = user.PhoneNumber;
        }

    }
}
