namespace CoastalPharmacyCRUD.DTOs
{
    public class ProductReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubCategoryId{ get; set; }
        public string CategoryDescription { get; set; }
        public string SubCategoryDescription { get; set; }
    }
}
