using System.ComponentModel.DataAnnotations;

namespace CoastalPharmacyCRUD.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email format incorrect")]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must contain at least 8 characters")]
        public string Password { get; set; }

    }
}