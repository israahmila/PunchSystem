using PunchSystem.Helpers;

namespace PunchSystem.Models;

public class Poincon:AuditableEntity
{
    public string Id { get; set; } = IdGenerator.New("POI");
    public string CodeFormat { get; set; } = string.Empty;
    public string Forme { get; set; } = string.Empty;
    public string MarqueId { get; set; } = string.Empty;
    public Marque Marque { get; set; } =new Marque();
    public string CodeGMAO { get; set; } = string.Empty;
    public string FournisseurId { get; set; } = string.Empty;
    public Fournisseur Fournisseur { get; set; } = new Fournisseur();
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

    public string Statut { get; set; } = "actif";
    public ICollection<Utilisation> Utilisations { get; set; } = new List<Utilisation>();
}
