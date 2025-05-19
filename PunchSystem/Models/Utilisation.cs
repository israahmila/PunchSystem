namespace PunchSystem.Models
{
    public class Utilisation
    {
        public int Id { get; set; } // N° Utilisation
        public DateTime DateUtilisation { get; set; } // Date
        public string? Compresseuse { get; set; } // Compresseuse
        public int NombreComprimés { get; set; } // Nombre Comprimés
        public string? EmplacementRetour { get; set; } // Emplacement Retour
        public string? Commentaire { get; set; } // Commentaire
        public List<Lot> Lots { get; set; } = new List<Lot>(); // One-to-Many
        public List<Poincon> Poincons { get; set; } = new List<Poincon>(); // Many-to-Many
        public List<User> Users { get; set; } = new List<User>(); // Many-to-Many
        public bool IsDeleted { get; set; } = false;

    }
}
