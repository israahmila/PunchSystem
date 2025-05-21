using PunchSystem.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace PunchSystem.Models
{
    public class Permission: AuditableEntity
    {
        public string Id { get; set; } = IdGenerator.New("PER");
        public string Module { get; set; } =string.Empty;
        public string Action { get; set; } = string.Empty;

        public ICollection<RolePermission> RolePermissions { get; set; }= new List<RolePermission>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

        [NotMapped]
        public string Name => $"{Module ?? ""}.{Action ?? ""}";


    }

}
