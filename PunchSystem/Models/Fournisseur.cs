using PunchSystem.Helpers;

namespace PunchSystem.Models
{
    public class Fournisseur : AuditableEntity
    {
        public string Id { get; set; } = IdGenerator.New("FOU");
        public string Code { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Pays { get; set; } = string.Empty;
        public string Statut { get; set; } = "Actif";
    }
}
