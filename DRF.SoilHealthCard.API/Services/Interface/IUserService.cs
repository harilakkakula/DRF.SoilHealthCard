using DRF.SoilHealthCard.API.DTO;
using DRF.SoilHealthCard.API.ViewModel;

namespace DRF.SoilHealthCard.API.Services.Interface
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(string username, string password,string ipAddress);
        Task<RegisterResponse> RegisterAsync(RegisterRequestDto model);
    }
}
