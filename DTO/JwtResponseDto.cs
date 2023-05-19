using System;

namespace SubsAPI.DTO
{
    public class JwtResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
