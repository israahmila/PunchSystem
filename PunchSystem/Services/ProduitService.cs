using Microsoft.EntityFrameworkCore;
using PunchSystem.Contracts;
using PunchSystem.Data;
using PunchSystem.Helpers;
using PunchSystem.Models;
using PunchSystem.Services;

public class ProduitService : IProduitService
{
    private readonly AppDbContext _context;
    private readonly IUserContextService _user;

    public ProduitService(AppDbContext context, IUserContextService user)
    {
        _context = context;
        _user = user;
    }

    public async Task<IEnumerable<Produit>> GetAllAsync()
    {
        return await _context.Produits
            .Where(p => p.Statut == "Actif")
            .ToListAsync();
    }

    public async Task<Produit?> GetByIdAsync(string id)
    {
        return await _context.Produits.FindAsync(id);
    }

    public async Task<Produit> CreateAsync(Produit produit)
    {
        produit.Id = IdGenerator.New("PRD");
        produit.CreatedBy = _user.GetCurrentUserId();
        _context.Produits.Add(produit);
        await _context.SaveChangesAsync();
        return produit;
    }

    public async Task<bool> UpdateAsync(string id, Produit updated, string raison)
    {
        var produit = await _context.Produits.FindAsync(id);
        if (produit == null) return false;

        produit.Code = updated.Code;
        produit.Designation = updated.Designation;
        produit.Statut = updated.Statut;
        produit.UpdatedAt = DateTime.UtcNow;
        produit.UpdatedBy = _user.GetCurrentUserId();

        _context.AuditTrails.Add(new AuditTrail
        {
            Id = IdGenerator.New("AUD"),
            Module = "Produit",
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
        var produit = await _context.Produits.FindAsync(id);
        if (produit == null) return false;

        produit.Statut = "Inactif";
        produit.UpdatedAt = DateTime.UtcNow;
        produit.UpdatedBy = _user.GetCurrentUserId();

        _context.AuditTrails.Add(new AuditTrail
        {
            Id = IdGenerator.New("AUD"),
            Module = "Produit",
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
