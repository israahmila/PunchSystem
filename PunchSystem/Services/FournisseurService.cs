using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.Helpers;
using PunchSystem.Models;

namespace PunchSystem.Services
{
    public class FournisseurService : IFournisseurService
    {
        private readonly AppDbContext _context;

        public FournisseurService(AppDbContext context)
        {
            _context = context;
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

        public async Task CreateAsync(Fournisseur fournisseur)
        {
            fournisseur.Id = IdGenerator.New("FOU");
            _context.Fournisseurs.Add(fournisseur);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, Fournisseur fournisseur)
        {
            var existing = await _context.Fournisseurs.FindAsync(id);
            if (existing == null) return;

            existing.Code = fournisseur.Code;
            existing.Designation = fournisseur.Designation;
            existing.Pays = fournisseur.Pays;
            existing.Statut = fournisseur.Statut;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(string id, string raison, string utilisateur)
        {
            var fournisseur = await _context.Fournisseurs.FindAsync(id);
            if (fournisseur == null) return;

            fournisseur.Statut = "Inactif";
            fournisseur.UpdatedAt = DateTime.UtcNow;

            _context.AuditTrails.Add(new AuditTrail
            {
                Id = IdGenerator.New("AUD"),
                Module = "Fournisseur",
                Action = "DELETE",
                ReferenceObjet = fournisseur.Id,
                Date = DateTime.UtcNow,
                Utilisateur = utilisateur,
                Raison = raison
            });

            await _context.SaveChangesAsync();
        }
    }

}
