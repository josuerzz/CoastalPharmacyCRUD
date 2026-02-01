namespace CoastalPharmacyCRUD.Models
{
    public class OBJ_Product
    {
        // OBJ : Objects
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public CDL_Identifier Category { get; set; }
        public Guid? SubCategoryId { get; set; }
        public CDL_Identifier SubCategory { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreateUserId { get; set; }
        public SYS_User CreateUser { get; set; }
        public OBJ_Product() { }
    }
}
