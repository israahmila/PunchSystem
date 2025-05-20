namespace PunchSystem.Models
{
    public class RolePermission:AuditableEntity
    {
        public string RoleId { get; set; } = string.Empty;
        public Role Role { get; set; } = null!;

        public string PermissionId { get; set; } = string.Empty;
        public Permission Permission { get; set; } = null!;
    }
}
