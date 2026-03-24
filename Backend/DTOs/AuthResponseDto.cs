using CoastalPharmacyCRUD.Models;

namespace CoastalPharmacyCRUD.DTOs
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public RefreshToken RefreshToken { get; set; } = new();
        public string? Error { get; set; }
    }
}
