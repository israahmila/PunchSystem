using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.Helpers;
using PunchSystem.Models;

namespace PunchSystem.Services
{
    public class EntretienService : IEntretienService
    {
        private readonly AppDbContext _context;

        public EntretienService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entretien>> GetAllAsync()
        {
            return await _context.Entretiens
                .Include(e => e.User)
                .ToListAsync();
        }

        public async Task<Entretien?> GetByIdAsync(string id)
        {
            return await _context.Entretiens
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(Entretien entretien)
        {
            entretien.Id = IdGenerator.New("ENT");
            _context.Entretiens.Add(entretien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, Entretien entretien)
        {
            var existing = await _context.Entretiens.FindAsync(id);
            if (existing == null) return;

            existing.Reference = entretien.Reference;
            existing.DateEntretien = entretien.DateEntretien;
            existing.UserId = entretien.UserId;
            existing.ReferenceUtilisation = entretien.ReferenceUtilisation;
            existing.Type = entretien.Type;
            existing.Commentaire = entretien.Commentaire;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string raison, string utilisateur)
        {
            var entretien = await _context.Entretiens.FindAsync(id);
            if (entretien == null) return;

            _context.Entretiens.Remove(entretien);

            _context.AuditTrails.Add(new AuditTrail
            {
                Id = IdGenerator.New("AUD"),
                Module = "Entretien",
                Action = "DELETE",
                ReferenceObjet = entretien.Id,
                Date = DateTime.UtcNow,
                Utilisateur = utilisateur,
                Raison = raison
            });

            await _context.SaveChangesAsync();
        }
    }

}
