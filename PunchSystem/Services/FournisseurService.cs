using Microsoft.EntityFrameworkCore;
using PunchSystem.Contracts;
using PunchSystem.Data;
using PunchSystem.Helpers;
using PunchSystem.Models;

public class FournisseurService : IFournisseurService
{
    private readonly AppDbContext _context;
    private readonly IUserContextService _user;

    public FournisseurService(AppDbContext context, IUserContextService user)
    {
        _context = context;
        _user = user;
    }

    public async Task<IEnumerable<Fournisseur>> GetAllAsync()
    {
        return await _context.Fournisseurs
            .Where(f => f.Statut == "Actif")
            .ToListAsync();
    }

    public async Task<Fournisseur?> GetByIdAsync(string id)
    {
        return await _context.Fournisseurs.FindAsync(id);
    }

    public async Task<Fournisseur> CreateAsync(Fournisseur fournisseur)
    {
        fournisseur.Id = IdGenerator.New("FRN");
        fournisseur.CreatedBy = _user.GetCurrentUserId();
        _context.Fournisseurs.Add(fournisseur);
        await _context.SaveChangesAsync();
        return fournisseur;
    }

    public async Task<bool> UpdateAsync(string id, Fournisseur updated, string raison)
    {
        var fournisseur = await _context.Fournisseurs.FindAsync(id);
        if (fournisseur == null) return false;

        fournisseur.Code = updated.Code;
        fournisseur.Designation = updated.Designation;
        fournisseur.Pays = updated.Pays;
        fournisseur.Statut = updated.Statut;
        fournisseur.UpdatedAt = DateTime.UtcNow;
        fournisseur.UpdatedBy = _user.GetCurrentUserId();

        _context.AuditTrails.Add(new AuditTrail
        {
            Id = IdGenerator.New("AUD"),
            Module = "Fournisseur",
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
        var fournisseur = await _context.Fournisseurs.FindAsync(id);
        if (fournisseur == null) return false;

        fournisseur.Statut = "Inactif";
        fournisseur.UpdatedAt = DateTime.UtcNow;
        fournisseur.UpdatedBy = _user.GetCurrentUserId();

        _context.AuditTrails.Add(new AuditTrail
        {
            Id = IdGenerator.New("AUD"),
            Module = "Fournisseur",
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
