using Microsoft.EntityFrameworkCore;
using PunchSystem.Contracts;
using PunchSystem.Data;
using PunchSystem.Models;

public class StatsService : IStatsService
{
    private readonly AppDbContext _context;

    public StatsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetUtilisationsStatsAsync()
    {
        var stats = await _context.Utilisations
            .GroupBy(u => u.DateUtilisation.Date)
            .Select(g => new
            {
                Date = g.Key,
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return stats;
    }

    public async Task<object> GetEtatPoinconsAsync()
    {
        var stats = await _context.Poincons
            .GroupBy(p => p.Statut)
            .Select(g => new
            {
                Statut = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        return stats;
    }

    public async Task<object> GetGlobalStatsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var totalProduits = await _context.Produits.CountAsync();
        var totalPoincons = await _context.Poincons.CountAsync();
        var totalUtilisations = await _context.Utilisations.CountAsync();
        var totalEntretiens = await _context.Entretiens.CountAsync();

        return new
        {
            Utilisateurs = totalUsers,
            Produits = totalProduits,
            Poincons = totalPoincons,
            Utilisations = totalUtilisations,
            Entretiens = totalEntretiens
        };
    }
}
