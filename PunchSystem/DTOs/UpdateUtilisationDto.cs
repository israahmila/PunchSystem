using PunchSystem.Models;

namespace PunchSystem.DTOs
{
    public class UpdateUtilisationDto
    {
        public string Reference { get; set; } = string.Empty;
        public DateTime DateUtilisation { get; set; }
        public string Comprimeuse { get; set; } = string.Empty;
        public string ProduitId { get; set; } = string.Empty;
        public string CodeFormatPoincon { get; set; } = string.Empty;
        public int NombreComprimés { get; set; }
        public string? EmplacementRetour { get; set; }
        public string? Commentaire { get; set; }

        public List<string> PoinconIds { get; set; } = new();
        public List<string> UserIds { get; set; } = new();
        public List<Lot> Lots { get; set; } = new();
    }
}
