﻿using DRF.SoilHealthCard.API.Model;

namespace DRF.SoilHealthCard.API.Utils.Service
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
        public int? ValidateJwtToken(string token);
        public RefreshToken GenerateRefreshToken(string ipAddress);
    }
}
