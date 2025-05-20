using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.Helpers;
using PunchSystem.Models;

namespace PunchSystem.Services
{
    public class PoinconService : IPoinconService
    {
        private readonly AppDbContext _context;

        public PoinconService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Poincon>> GetAllAsync()
        {
            return await _context.Poincons
                .Where(p => p.Statut == "Actif")
                .Include(p => p.Fournisseur)
                .Include(p => p.Marque)
                .ToListAsync();
        }

        public async Task<Poincon?> GetByIdAsync(string id)
        {
            return await _context.Poincons
                .Include(p => p.Fournisseur)
                .Include(p => p.Marque)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(Poincon poincon)
        {
            poincon.Id = IdGenerator.New("POI");
            _context.Poincons.Add(poincon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, Poincon poincon)
        {
            var existing = await _context.Poincons.FindAsync(id);
            if (existing == null) return;

            existing.CodeFormat = poincon.CodeFormat;
            existing.Forme = poincon.Forme;
            existing.FournisseurId = poincon.FournisseurId;
            existing.MarqueId = poincon.MarqueId;
            existing.FicheTechniqueUrl = poincon.FicheTechniqueUrl;
            existing.Statut = poincon.Statut;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(string id, string raison, string utilisateur)
        {
            var poincon = await _context.Poincons.FindAsync(id);
            if (poincon == null) return;

            poincon.Statut = "Inactif";
            poincon.UpdatedAt = DateTime.UtcNow;

            _context.AuditTrails.Add(new AuditTrail
            {
                Id = IdGenerator.New("AUD"),
                Module = "Poincon",
                Action = "DELETE",
                ReferenceObjet = poincon.Id,
                Date = DateTime.UtcNow,
                Utilisateur = utilisateur,
                Raison = raison
            });

            await _context.SaveChangesAsync();
        }
    }

}
