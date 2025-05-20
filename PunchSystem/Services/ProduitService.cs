using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.Helpers;
using PunchSystem.Models;

namespace PunchSystem.Services
{
    public class ProduitService : IProduitService
    {
        private readonly AppDbContext _context;

        public ProduitService(AppDbContext context)
        {
            _context = context;
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

        public async Task CreateAsync(Produit produit)
        {
            produit.Id = IdGenerator.New("PRD");
            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, Produit produit)
        {
            var existing = await _context.Produits.FindAsync(id);
            if (existing == null) return;

            existing.Code = produit.Code;
            existing.Designation = produit.Designation;
            existing.Statut = produit.Statut;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(string id, string raison, string utilisateur)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null) return;

            produit.Statut = "Inactif";
            produit.UpdatedAt = DateTime.UtcNow;

            _context.AuditTrails.Add(new AuditTrail
            {
                Id = IdGenerator.New("AUD"),
                Module = "Produit",
                Action = "DELETE",
                ReferenceObjet = produit.Id,
                Date = DateTime.UtcNow,
                Utilisateur = utilisateur,
                Raison = raison
            });

            await _context.SaveChangesAsync();
        }

        public Task SoftDeleteAsync(string id, string raison)
        {
            throw new NotImplementedException();
        }
    }


}
