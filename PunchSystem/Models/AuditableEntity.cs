namespace PunchSystem.Models
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set;} = null!;

    }
}
