using Microsoft.EntityFrameworkCore;
using PunchSystem.Contracts;
using PunchSystem.Data;
using PunchSystem.Helpers;
using PunchSystem.Models;

public class MarqueService : IMarqueService
{
    private readonly AppDbContext _context;
    private readonly IUserContextService _user;

    public MarqueService(AppDbContext context, IUserContextService user)
    {
        _context = context;
        _user = user;
    }

    public async Task<IEnumerable<Marque>> GetAllAsync()
    {
        return await _context.Marques
            .Where(m => m.Statut == "Actif")
            .ToListAsync();
    }

    public async Task<Marque?> GetByIdAsync(string id)
    {
        return await _context.Marques.FindAsync(id);
    }

    public async Task<Marque> CreateAsync(Marque marque)
    {
        marque.Id = IdGenerator.New("MRQ");
        marque.CreatedBy = _user.GetCurrentUserId();
        _context.Marques.Add(marque);
        await _context.SaveChangesAsync();
        return marque;
    }

    public async Task<bool> UpdateAsync(string id, Marque updated, string raison)
    {
        var marque = await _context.Marques.FindAsync(id);
        if (marque == null) return false;

        marque.Code = updated.Code;
        marque.Designation = updated.Designation;
        marque.Statut = updated.Statut;
        marque.UpdatedAt = DateTime.UtcNow;
        marque.UpdatedBy = _user.GetCurrentUserId();

        _context.AuditTrails.Add(new AuditTrail
        {
            Id = IdGenerator.New("AUD"),
            Module = "Marque",
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
        var marque = await _context.Marques.FindAsync(id);
        if (marque == null) return false;

        marque.Statut = "Inactif";
        marque.UpdatedAt = DateTime.UtcNow;
        marque.UpdatedBy = _user.GetCurrentUserId();

        _context.AuditTrails.Add(new AuditTrail
        {
            Id = IdGenerator.New("AUD"),
            Module = "Marque",
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
