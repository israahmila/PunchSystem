using PunchSystem.Helpers;

namespace PunchSystem.Models
{
    public class Role:AuditableEntity
    {
        public string Id { get; set; } = IdGenerator.New("ROL");
        public string Name { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } = [];
        public ICollection<User> Users { get; set; } = [];
    }
}
