using System.ComponentModel.DataAnnotations;

namespace CoastalPharmacyCRUD.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email format incorrect")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "The password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
