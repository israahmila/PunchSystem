using PunchSystem.Helpers;

namespace PunchSystem.Models
{    
    public class AuditTrail
    {
        public string Id { get; set; } = IdGenerator.New("AUD");
        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string ReferenceObjet { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Utilisateur { get; set; } = string.Empty;
        public string Raison { get; set; } = string.Empty;
    }

}
