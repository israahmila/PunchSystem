using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.Security;

namespace PunchSystem.Controllers
{
    [Authorize(Policy = PermissionPolicies.ViewStats)]
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatsController(AppDbContext context)
        {
            _context = context;
        }

        // 📊 Utilisations par jour (sur 30 jours)
        [HttpGet("utilisations-par-jour")] // ✅ Renamed route to prevent conflict
        public async Task<IActionResult> GetUtilisationsParJour()
        {
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-30);

            var data = await _context.Utilisations
                .Where(u => u.DateUtilisation >= startDate && u.DateUtilisation <= endDate)
                .GroupBy(u => u.DateUtilisation.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Total = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToListAsync();

            return Ok(data);
        }

        // 📊 État des poinçons par Statut
        [HttpGet("etat-poincon")]
        public async Task<IActionResult> GetEtatPoincons()
        {
            var data = await _context.Poincons
                .GroupBy(p => p.Statut)
                .Select(g => new
                {
                    Statut = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            return Ok(data);
        }

        // 📊 Statistiques globales
        [HttpGet("globale")]
        public async Task<IActionResult> GetStatsGlobales()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalProduits = await _context.Produits.CountAsync();
            var totalPoincons = await _context.Poincons.CountAsync();
            var totalUtilisations = await _context.Utilisations.CountAsync();

            return Ok(new
            {
                Utilisateurs = totalUsers,
                Produits = totalProduits,
                Poincons = totalPoincons,
                Utilisations = totalUtilisations
            });
        }
    }
}
