namespace PunchSystem.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public int UtilisationId { get; set; }
        public string? Produit { get; set; } // Produits
        public string LotNumber { get; set; } // N° Lots
        public Utilisation Utilisation { get; set; }
    }
}
