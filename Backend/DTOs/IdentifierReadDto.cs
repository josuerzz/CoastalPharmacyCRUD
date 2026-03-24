namespace CoastalPharmacyCRUD.DTOs
{
    public class IdentifierReadDto
    {
        public Guid Id { get; set; }
        public string Set { get; set; }
        public string Description { get; set; }
        public Guid ParentId { get; set; }
    }
}
