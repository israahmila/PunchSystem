using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.DTOs;
using PunchSystem.Helpers;
using PunchSystem.Models;

namespace PunchSystem.Services
{
    public class UtilisationService : IUtilisationService
    {
        private readonly AppDbContext _context;

        public UtilisationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Utilisation>> GetAllAsync()
        {
            return await _context.Utilisations
                .Include(u => u.Produit)
                .Include(u => u.Users)
                .ToListAsync();
        }

        public async Task<Utilisation?> GetByIdAsync(string id)
        {
            return await _context.Utilisations
                .Include(u => u.Produit)
                .Include(u => u.Users)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Utilisation> CreateAsync(CreateUtilisationDto dto)
        {
            var utilisation = new Utilisation
            {
                Id = IdGenerator.New("UTL"),
                Comprimeuse = dto.Comprimeuse ?? string.Empty,
                NombreComprimés = dto.NombreComprimés,
                EmplacementRetour = dto.EmplacementRetour,
                Commentaire = dto.Commentaire,
                DateUtilisation = DateTime.UtcNow, // ou un champ dans le DTO si fourni
                Reference = "AUTO-GEN", // ou génère avec Id, Date, etc.
                CodeFormatPoincon = string.Empty,
                ProduitId = "", // Si manquant, tu peux l’ajouter dans le DTO
            };

            // Charger Users
            foreach (var userId in dto.UserIds)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                    utilisation.Users.Add(user);
            }

            // Charger Poinçons
            foreach (var poinconId in dto.PoinconIds)
            {
                var poincon = await _context.Poincons.FindAsync(poinconId);
                if (poincon != null)
                    utilisation.Poincons.Add(poincon);
            }

            // Créer les Lots
            foreach (var lotNumber in dto.LotNumbers)
            {
                utilisation.Lots.Add(new Lot
                {
                    Id = IdGenerator.New("LOT"),
                    LotNumber = lotNumber,
                    Produit = "Auto", // ou à ajouter dans DTO
                    Utilisation = utilisation
                });
            }

            _context.Utilisations.Add(utilisation);
            await _context.SaveChangesAsync();
            return utilisation;
        }


        public async Task<bool> UpdateAsync(string id, UpdateUtilisationDto dto)
        {
            var existing = await _context.Utilisations
                .Include(u => u.Poincons)
                .Include(u => u.Users)
                .Include(u => u.Lots)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (existing == null)
                return false;


            // Propriétés simples
            existing.Reference = dto.Reference;
            existing.DateUtilisation = dto.DateUtilisation;
            existing.Comprimeuse = dto.Comprimeuse;
            existing.ProduitId = dto.ProduitId;
            existing.CodeFormatPoincon = dto.CodeFormatPoincon;
            existing.NombreComprimés = dto.NombreComprimés;
            existing.EmplacementRetour = dto.EmplacementRetour;
            existing.Commentaire = dto.Commentaire;
            existing.UpdatedAt = DateTime.UtcNow;

            // 🎯 Replace Poincons
            existing.Poincons.Clear();
            foreach (var poinconId in dto.PoinconIds)
            {
                var poincon = await _context.Poincons.FindAsync(poinconId);
                if (poincon != null)
                    existing.Poincons.Add(poincon);
            }

            // 🎯 Replace Users
            existing.Users.Clear();
            foreach (var userId in dto.UserIds)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                    existing.Users.Add(user);
            }

            // 🎯 Replace Lots
            _context.Lots.RemoveRange(existing.Lots);
            foreach (var lot in dto.Lots)
            {
                lot.Id = IdGenerator.New("LOT");
                existing.Lots.Add(lot);
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task DeleteAsync(string id, string raison, string utilisateur)
        {
            var utilisation = await _context.Utilisations.FindAsync(id);
            if (utilisation == null) return;

            _context.Utilisations.Remove(utilisation);

            _context.AuditTrails.Add(new AuditTrail
            {
                Id = IdGenerator.New("AUD"),
                Module = "Utilisation",
                Action = "DELETE",
                ReferenceObjet = utilisation.Id,
                Date = DateTime.UtcNow,
                Utilisateur = utilisateur,
                Raison = raison
            });

            await _context.SaveChangesAsync();
        }

        
    }

}