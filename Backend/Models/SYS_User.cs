namespace CoastalPharmacyCRUD.Models
{
    public class SYS_User
    {
        // SYS : System
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Status { get; set; }
        public Guid RoleIdentifierId { get; set; }
        public CDL_Identifier RoleIdentifier { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

    }
}
