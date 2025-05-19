using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.DTOs;
using PunchSystem.Models;
using System.Collections.Generic;

namespace PunchSystem.Services
{
    public class UtilisationService : IUtilisationService
    {
        private readonly AppDbContext _context;

        public UtilisationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UtilisationDto>> GetAllAsync()
        {
            return await _context.Utilisations
                .Include(u => u.Lots)
                .Include(u => u.Poincons)
                .Include(u => u.Users)
                .Select(u => new UtilisationDto
                {
                    Id = u.Id,
                    DateUtilisation = u.DateUtilisation,
                    Compresseuse = u.Compresseuse,
                    NombreComprimés = u.NombreComprimés,
                    EmplacementRetour = u.EmplacementRetour,
                    Commentaire = u.Commentaire,
                    LotNumbers = u.Lots.Select(l => l.LotNumber).ToList(),
                    PoinconIds = u.Poincons.Select(p => p.Id).ToList(),
                    CodeFormats = u.Poincons.Select(p => p.CodeFormat).ToList(),
                    EtatPoincons = u.Poincons.Select(p => p.Status).ToList(),
                    UserIds = u.Users.Select(u => u.Id).ToList()
                })
                .ToListAsync();
        }

        public async Task<UtilisationDto> CreateAsync(CreateUtilisationDto dto)
        {
            var utilisation = new Utilisation
            {
                Compresseuse = dto.Compresseuse,
                NombreComprimés = dto.NombreComprimés,
                EmplacementRetour = dto.EmplacementRetour,
                Commentaire = dto.Commentaire,
                DateUtilisation = DateTime.UtcNow
            };

            var poincons = await _context.Poincons
                .Where(p => dto.PoinconIds.Contains(p.Id))
                .ToListAsync();
            utilisation.Poincons = poincons;

            var lots = dto.LotNumbers.Select(ln => new Lot
            {
                LotNumber = ln,
                Produit = dto.Compresseuse
            }).ToList();
            utilisation.Lots = lots;

            var utilisateurs = await _context.Users
                .Where(u => dto.UserIds.Contains(u.Id))
                .ToListAsync();
            utilisation.Users = utilisateurs;

            _context.Utilisations.Add(utilisation);
            await _context.SaveChangesAsync();

            return new UtilisationDto
            {
                Id = utilisation.Id,
                DateUtilisation = utilisation.DateUtilisation,
                Compresseuse = utilisation.Compresseuse,
                NombreComprimés = utilisation.NombreComprimés,
                EmplacementRetour = utilisation.EmplacementRetour,
                Commentaire = utilisation.Commentaire,
                LotNumbers = utilisation.Lots.Select(l => l.LotNumber).ToList(),
                PoinconIds = utilisation.Poincons.Select(p => p.Id).ToList(),
                CodeFormats = utilisation.Poincons.Select(p => p.CodeFormat).ToList(),
                EtatPoincons = utilisation.Poincons.Select(p => p.Status).ToList(),
                UserIds = utilisation.Users.Select(u => u.Id).ToList()
            };
        }

        public async Task<UtilisationDto?> GetByIdAsync(int id)
        {
            var u = await _context.Utilisations
                .Include(u => u.Poincons)
                .Include(u => u.Users)
                .Include(u => u.Lots)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (u == null) return null;

            return new UtilisationDto
            {
                Id = u.Id,
                DateUtilisation = u.DateUtilisation,
                Compresseuse = u.Compresseuse,
                NombreComprimés = u.NombreComprimés,
                EmplacementRetour = u.EmplacementRetour,
                Commentaire = u.Commentaire,
                LotNumbers = u.Lots.Select(l => l.LotNumber).ToList(),
                PoinconIds = u.Poincons.Select(p => p.Id).ToList(),
                CodeFormats = u.Poincons.Select(p => p.CodeFormat).ToList(),
                EtatPoincons = u.Poincons.Select(p => p.Status).ToList(),
                UserIds = u.Users.Select(u => u.Id).ToList()
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateUtilisationDto dto)
        {
            var utilisation = await _context.Utilisations
                .Include(u => u.Poincons)
                .Include(u => u.Users)
                .Include(u => u.Lots)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (utilisation == null)
                return false;

            utilisation.DateUtilisation = dto.DateUtilisation;
            utilisation.Compresseuse = dto.Compresseuse;
            utilisation.NombreComprimés = dto.NombreComprimés;
            utilisation.EmplacementRetour = dto.EmplacementRetour;
            utilisation.Commentaire = dto.Commentaire;

            var poincons = await _context.Poincons
                .Where(p => dto.PoinconIds.Contains(p.Id))
                .ToListAsync();
            utilisation.Poincons.Clear();
            foreach (var p in poincons)
                utilisation.Poincons.Add(p);

            var users = await _context.Users
                .Where(u => dto.UserIds.Contains(u.Id))
                .ToListAsync();
            utilisation.Users.Clear();
            foreach (var u in users)
                utilisation.Users.Add(u);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var utilisation = await _context.Utilisations.FindAsync(id);

            if (utilisation == null || utilisation.IsDeleted)
                return false;

            utilisation.IsDeleted = true;
            _context.Utilisations.Update(utilisation);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}