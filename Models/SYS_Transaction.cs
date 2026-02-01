using System.ComponentModel.DataAnnotations.Schema;

namespace CoastalPharmacyCRUD.Models
{
    public class SYS_Transaction
    {
        // SYS : System
        public Guid Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NumberTransaction { get; set; }  
        public Guid ProcessId { get; set; }
        public CDL_Identifier Process { get; set; }
        public Guid UserId{ get; set; }
        public SYS_User User { get; set; }
        // Affected tbl name
        public string EntityName { get; set; }

        // Register Id changed
        public Guid EntityId { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; }

    }
}
