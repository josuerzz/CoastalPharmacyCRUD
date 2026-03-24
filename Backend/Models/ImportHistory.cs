using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoastalPharmacyCRUD.Models
{
    public enum CodeImportStatus
    {
        Success = 1,
        Failed = 0
    }
    public class ImportHistory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; } = string.Empty;
        public int TotalRecords { get; set; }
        public string? ErrorMessage { get; set; }
        [Required]
        public DateTime ImportDate { get; set; }
        public CodeImportStatus ImportStatus { get; set; }
        [Required]
        public Guid ImportUserId { get; set; }
        [JsonIgnore]
        public SYS_User ImportUser { get; set; }
    }
}