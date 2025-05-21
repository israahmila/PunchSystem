using Microsoft.EntityFrameworkCore;
using PunchSystem.Contracts;
using PunchSystem.Data;
using PunchSystem.Helpers;
using PunchSystem.Models;

public class PoinconService : IPoinconService
{
    private readonly AppDbContext _context;
    private readonly IUserContextService _user;

    public PoinconService(AppDbContext context, IUserContextService user)
    {
        _context = context;
        _user = user;
    }

    public async Task<IEnumerable<Poincon>> GetAllAsync()
    {
        return await _context.Poincons
            .Include(p => p.Fournisseur)
            .Include(p => p.Marque)
            .Where(p => p.Statut == "Actif")
            .ToListAsync();
    }

    public async Task<Poincon?> GetByIdAsync(string id)
    {
        return await _context.Poincons
            .Include(p => p.Fournisseur)
            .Include(p => p.Marque)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Poincon> CreateAsync(Poincon poincon)
    {
        poincon.Id = IdGenerator.New("PNC");
        poincon.CreatedBy = _user.GetCurrentUserId();
        _context.Poincons.Add(poincon);
        await _context.SaveChangesAsync();
        return poincon;
    }

    public async Task<bool> UpdateAsync(string id, Poincon updated, string raison)
    {
        var poincon = await _context.Poincons.FindAsync(id);
        if (poincon == null) return false;

        poincon.CodeFormat = updated.CodeFormat;
        poincon.Forme = updated.Forme;
        poincon.CodeGMAO = updated.CodeGMAO;
        poincon.GravureSup = updated.GravureSup;
        poincon.GravureInf = updated.GravureInf;
        poincon.FicheTechniqueUrl = updated.FicheTechniqueUrl;
        poincon.Clavetage = updated.Clavetage;
        poincon.Secabilite = updated.Secabilite;
        poincon.EmplacementReception = updated.EmplacementReception;
        poincon.DateReception = updated.DateReception;
        poincon.DateFabrication = updated.DateFabrication;
        poincon.DateMiseEnService = updated.DateMiseEnService;
        poincon.Commentaire = updated.Commentaire;
        poincon.Matrice = updated.Matrice;
        poincon.RefSup = updated.RefSup;
        poincon.RefInf = updated.RefInf;
        poincon.Largeur = updated.Largeur;
        poincon.Longueur = updated.Longueur;
        poincon.Diametre = updated.Diametre;
        poincon.ChAdm = updated.ChAdm;
        poincon.FournisseurId = updated.FournisseurId;
        poincon.MarqueId = updated.MarqueId;
        poincon.UpdatedAt = DateTime.UtcNow;
        poincon.UpdatedBy = _user.GetCurrentUserId();

        _context.AuditTrails.Add(new AuditTrail
        {
            Id = IdGenerator.New("AUD"),
            Module = "Poincon",
            Action = "UPDATE",
            ReferenceObjet = id,
            Date = DateTime.UtcNow,
            Utilisateur = _user.GetCurrentUsername(),
            Raison = raison
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SoftDeleteAsync(string id, string raison)
    {
        var poincon = await _context.Poincons.FindAsync(id);
        if (poincon == null) return false;

        poincon.Statut = "Inactif";
        poincon.UpdatedAt = DateTime.UtcNow;
        poincon.UpdatedBy = _user.GetCurrentUserId();

        _context.AuditTrails.Add(new AuditTrail
        {
            Id = IdGenerator.New("AUD"),
            Module = "Poincon",
            Action = "DELETE",
            ReferenceObjet = id,
            Date = DateTime.UtcNow,
            Utilisateur = _user.GetCurrentUsername(),
            Raison = raison
        });

        await _context.SaveChangesAsync();
        return true;
    }
}
