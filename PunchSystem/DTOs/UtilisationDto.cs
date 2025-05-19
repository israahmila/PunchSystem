namespace PunchSystem.DTOs
{
    public class UtilisationDto
    {
        public int Id { get; set; }
        public DateTime DateUtilisation { get; set; }
        public string? Compresseuse { get; set; }
        public int NombreComprimés { get; set; }
        public string? EmplacementRetour { get; set; }
        public string? Commentaire { get; set; }
        public List<string> LotNumbers { get; set; } = new List<string>();
        public List<int> PoinconIds { get; set; } = new List<int>();
        public List<int> UserIds { get; set; } = new List<int>();
        public List<string> CodeFormats { get; set; } = new List<string>();
        public List<string> EtatPoincons { get; set; } = new List<string>();
    }

    public class CreateUtilisationDto
    {
        public string? Compresseuse { get; set; }
        public int NombreComprimés { get; set; }
        public string? EmplacementRetour { get; set; }
        public string? Commentaire { get; set; }
        public List<string> LotNumbers { get; set; } = new List<string>();
        public List<int> PoinconIds { get; set; } = new List<int>();
        public List<int> UserIds { get; set; } = new List<int>();
    }
}