namespace CoastalPharmacyCRUD.Models
{
    public class CDL_Identifier
    {
        // CDL : Codes List
        public Guid Id { get; set; }
        public string Set { get; set; }
        public int ElementNumber { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public string Use { get; set; }
        public Guid? ParentId { get; set; }
        public virtual CDL_Identifier? Parent { get; set; }
        public virtual ICollection<CDL_Identifier> Children { get; set; } = new List<CDL_Identifier>();
        public ICollection<OBJ_Product> Category { get; set; } = new List<OBJ_Product>();
        public ICollection<OBJ_Product> Subcategory { get; set; } = new List<OBJ_Product>();
        public ICollection<SYS_User> RoleIdentifier { get; set; } = new List<SYS_User>();
        public ICollection<SYS_Transaction> Process { get; set; } = new List<SYS_Transaction>();
    }
}
