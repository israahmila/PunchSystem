using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.Helpers;
using PunchSystem.Models;

namespace PunchSystem.Services
{
    public class MarqueService : IMarqueService
    {
        private readonly AppDbContext _context;

        public MarqueService(AppDbContext context)
        {
            _context = context;
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

        public async Task CreateAsync(Marque marque)
        {
            marque.Id = IdGenerator.New("MRQ");
            _context.Marques.Add(marque);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, Marque marque)
        {
            var existing = await _context.Marques.FindAsync(id);
            if (existing == null) return;

            existing.Code = marque.Code;
            existing.Designation = marque.Designation;
            existing.Statut = marque.Statut;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(string id, string raison, string utilisateur)
        {
            var marque = await _context.Marques.FindAsync(id);
            if (marque == null) return;

            marque.Statut = "Inactif";
            marque.UpdatedAt = DateTime.UtcNow;

            _context.AuditTrails.Add(new AuditTrail
            {
                Id = IdGenerator.New("AUD"),
                Module = "Marque",
                Action = "DELETE",
                ReferenceObjet = marque.Id,
                Date = DateTime.UtcNow,
                Utilisateur = utilisateur,
                Raison = raison
            });

            await _context.SaveChangesAsync();
        }
    }

}
