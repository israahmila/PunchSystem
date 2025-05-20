using PunchSystem.Helpers;

namespace PunchSystem.Models
{
    public class Marque:AuditableEntity
    {
        public string Id { get; set; } = IdGenerator.New("MRQ");
        public string Code { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Statut { get; set; } = "Actif";
    }
}
