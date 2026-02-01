using CoastalPharmacyCRUD.Models;

namespace CoastalPharmacyCRUD.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public RefreshToken RefreshToken { get; set; } = new();
    }
}
