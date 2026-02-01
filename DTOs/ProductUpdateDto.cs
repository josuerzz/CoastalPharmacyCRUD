using System.ComponentModel.DataAnnotations;
namespace CoastalPharmacyCRUD.DTOs
{
    public class ProductUpdateDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        [Range (1, int.MaxValue)]
        public int Stock { get; set; }
        [Range(500, int.MaxValue)]
        public decimal Value { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid SubCategoryId { get; set; }


    }
}
