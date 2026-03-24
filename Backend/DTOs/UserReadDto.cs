namespace CoastalPharmacyCRUD.DTOs
{
    public class UserReadDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string RoleCode { get; set; }
    }
}