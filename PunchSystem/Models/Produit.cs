using PunchSystem.Helpers;

namespace PunchSystem.Models
{
    public class Produit:AuditableEntity
    {
        public string Id { get; set; } = IdGenerator.New("PRD");
        public string Code { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Statut { get; set; } = "Actif";
    }

}
