using PunchSystem.Helpers;

namespace PunchSystem.Models
{
    public class Utilisation:AuditableEntity
    {
        public string Id { get; set; } = IdGenerator.New("UTL");
        
        public string Reference { get; set; } = string.Empty; // Référence
        public DateTime DateUtilisation { get; set; } // Date
        public string Comprimeuse { get; set; } = string.Empty;

        public string ProduitId { get; set; } = string.Empty;
        public Produit Produit { get; set; } = new Produit(); // Produits

        public string CodeFormatPoincon { get; set; } = string.Empty;
        public int NombreComprimés { get; set; } // Nombre Comprimés
        public string? EmplacementRetour { get; set; } // Emplacement Retour
        public string? Commentaire { get; set; } // Commentaire
        public List<Lot> Lots { get; set; } = new List<Lot>(); // One-to-Many
        public List<Poincon> Poincons { get; set; } = new List<Poincon>(); // Many-to-Many
        public List<User> Users { get; set; } = new List<User>(); // Many-to-Many
        

    }
}
