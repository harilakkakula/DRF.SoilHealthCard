namespace DRF.SoilHealthCard.API.DTO
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public bool IsUserCreated {get;set; }
    }
}
