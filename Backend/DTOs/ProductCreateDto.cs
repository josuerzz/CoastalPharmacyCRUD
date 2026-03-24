using System.ComponentModel.DataAnnotations;

namespace CoastalPharmacyCRUD.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid SubCategoryId { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public string Description { get; set; }

        [Range(500, double.MaxValue)]
        public decimal Value { get; set; }
    }
}