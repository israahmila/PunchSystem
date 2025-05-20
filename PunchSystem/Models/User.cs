using PunchSystem.Helpers;
using System.Reflection.Emit;

namespace PunchSystem.Models
{
    public class User : AuditableEntity
    {
        public string Id { get; set; } = IdGenerator.New("USR");
        public string Username { get; set; } = null!;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = null!;
        public string RoleId { get; set; }= string.Empty;
        public Role Role { get; set; } = null!;
        public string Statut { get; set; } = "Actif";

        public bool IsActive { get; set; } = true;
        public int FailedLoginAttempts { get; set; } = 0;

        public ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

        public ICollection<Utilisation> Utilisations { get; set; } = new List<Utilisation>();

    }
}
