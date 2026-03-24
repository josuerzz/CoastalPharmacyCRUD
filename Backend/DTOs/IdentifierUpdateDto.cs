using System.ComponentModel.DataAnnotations;

namespace CoastalPharmacyCRUD.DTOs
{
    public class IdentifierUpdateDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(3)]
        public string Set { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Description { get; set; }
    }
}
