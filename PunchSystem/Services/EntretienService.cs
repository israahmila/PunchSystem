using Microsoft.EntityFrameworkCore;
using PunchSystem.Contracts;
using PunchSystem.Data;
using PunchSystem.Helpers;
using PunchSystem.Models;

public class EntretienService : IEntretienService
{
    private readonly AppDbContext _context;
    private readonly IUserContextService _user;

    public EntretienService(AppDbContext context, IUserContextService user)
    {
        _context = context;
        _user = user;
    }

    public async Task<IEnumerable<Entretien>> GetAllAsync()
    {
        return await _context.Entretiens
            .Include(e => e.User)
            .OrderByDescending(e => e.DateEntretien)
            .ToListAsync();
    }

    public async Task<Entretien?> GetByIdAsync(string id)
    {
        return await _context.Entretiens
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Entretien> CreateAsync(Entretien entretien)
    {
        entretien.Id = IdGenerator.New("ENT");
        entretien.CreatedBy = _user.GetCurrentUserId();
        _context.Entretiens.Add(entretien);
        await _context.SaveChangesAsync();
        return entretien;
    }

    public async Task<bool> UpdateAsync(string id, Entretien updated, string raison)
    {
        var entretien = await _context.Entretiens.FindAsync(id);
        if (entretien == null) return false;

        entretien.Reference = updated.Reference;
        entretien.DateEntretien = updated.DateEntretien;
        entretien.UserId = updated.UserId;
        entretien.ReferenceUtilisation = updated.ReferenceUtilisation;
        entretien.Type = updated.Type;
        entretien.Commentaire = updated.Commentaire;
        entretien.UpdatedAt = DateTime.UtcNow;
        entretien.UpdatedBy = _user.GetCurrentUserId();

        _context.AuditTrails.Add(new AuditTrail
        {
            Id = IdGenerator.New("AUD"),
            Module = "Entretien",
            Action = "UPDATE",
            ReferenceObjet = id,
            Date = DateTime.UtcNow,
            Utilisateur = _user.GetCurrentUsername(),
            Raison = raison
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id, string raison)
    {
        var entretien = await _context.Entretiens.FindAsync(id);
        if (entretien == null) return false;

        _context.Entretiens.Remove(entretien);

        _context.AuditTrails.Add(new AuditTrail
        {
            Id = IdGenerator.New("AUD"),
            Module = "Entretien",
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
