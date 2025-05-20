namespace PunchSystem.DTOs
{
    public class UtilisationDto
    {
        public string Id { get; set; } = string.Empty;
        public DateTime DateUtilisation { get; set; }
        public string? Comprimeuse { get; set; }
        public int NombreComprimés { get; set; }
        public string? EmplacementRetour { get; set; }
        public string? Commentaire { get; set; }
        public List<string> LotNumbers { get; set; } = new List<string>();
        public List<string> PoinconIds { get; set; } = new(); 
        public List<string> UserIds { get; set; } = new();
        public List<string> CodeFormats { get; set; } = new List<string>();
        public List<string> EtatPoincons { get; set; } = new List<string>();
    }

    public class CreateUtilisationDto
    {
        public string? Comprimeuse { get; set; }
        public int NombreComprimés { get; set; }
        public string? EmplacementRetour { get; set; }
        public string? Commentaire { get; set; }

        public List<string> LotNumbers { get; set; } = new();     // ✅ initialise
        public List<string> PoinconIds { get; set; } = new();     // ✅ initialise
        public List<string> UserIds { get; set; } = new();        // ✅ initialise
    }

}