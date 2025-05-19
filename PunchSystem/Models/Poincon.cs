namespace PunchSystem.Models;

public class Poincon
{
    public int Id { get; set; } 
    public string CodeFormat { get; set; } = string.Empty;
    public string? Forme { get; set; }
    public string? Marque { get; set; }
    public string? CodeGMAO { get; set; }
    public string? Fournisseur { get; set; }
    public string? GravureSup { get; set; }
    public string? GravureInf { get; set; }
    public string? Secabilite { get; set; }
    public string? Clavetage { get; set; }
    public string? EmplacementReception { get; set; }
    public DateTime? DateReception { get; set; }
    public DateTime? DateFabrication { get; set; }
    public DateTime? DateMiseEnService { get; set; }
    public string? Commentaire { get; set; }
    public string? FicheTechniqueUrl { get; set; }
    public string? Matrice { get; set; }
    public double? Largeur { get; set; }
    public double? Longueur { get; set; }
    public string? RefSup { get; set; }
    public string? RefInf { get; set; }
    public double? Diametre { get; set; }
    public string? ChAdm { get; set; }

    public string Status { get; set; } = "actif";
    public List<Utilisation> Utilisations { get; set; } = new List<Utilisation>();
}
