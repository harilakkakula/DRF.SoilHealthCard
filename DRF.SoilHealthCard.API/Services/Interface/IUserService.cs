using DRF.SoilHealthCard.API.DTO;

namespace DRF.SoilHealthCard.API.Services.Interface
{
    public interface IUserService
    {
        string Authenticate(string username, string password);
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto model);
    }
}
