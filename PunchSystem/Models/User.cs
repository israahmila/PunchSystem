namespace PunchSystem.Models
{
    public class User : AuditableEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;


        public bool IsActive { get; set; } = true;
        public int FailedLoginAttempts { get; set; } = 0;

        public ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

        public List<Utilisation> Utilisations { get; set; } = new List<Utilisation>();

    }
}
