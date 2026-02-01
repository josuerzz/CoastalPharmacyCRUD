using System.ComponentModel.DataAnnotations;

namespace CoastalPharmacyCRUD.DTOs
{
    public class IdentifierCreateDto
    {
        [Required]
        [StringLength(3)]
        public string Set { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Description { get; set; }
        [Required]
        public string Use { get; set; }
        public Guid? ParentId { get; set; }
    }
}
