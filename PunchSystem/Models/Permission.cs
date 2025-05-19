namespace PunchSystem.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}
