using PunchSystem.Helpers;

namespace PunchSystem.Models
{
    public class Lot:AuditableEntity
    {
        public string Id { get; set; } = IdGenerator.New("LOT");
        public string UtilisationId { get; set; }=string.Empty;
        public Utilisation Utilisation { get; set; } = new ();
        public string? Produit { get; set; } // Produits
        public string LotNumber { get; set; } // N° Lots
    }
}
