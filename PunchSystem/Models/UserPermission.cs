namespace PunchSystem.Models
{
    public class UserPermission : AuditableEntity
    {
        public string UserId { get; set; }= string.Empty;
        public User User { get; set; } = null!;

        public string PermissionId { get; set; } = string.Empty;
        public Permission Permission { get; set; } = null!;
    }
}
