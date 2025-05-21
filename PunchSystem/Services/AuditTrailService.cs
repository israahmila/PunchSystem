using Microsoft.EntityFrameworkCore;
using PunchSystem.Contracts;
using PunchSystem.Data;
using PunchSystem.Models;

public class AuditTrailService : IAuditTrailService
{
    private readonly AppDbContext _context;

    public AuditTrailService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AuditTrail>> GetAllAsync(string? module = null, string? utilisateur = null, DateTime? from = null, DateTime? to = null)
    {
        var query = _context.AuditTrails.AsQueryable();

        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(a => a.Module == module);

        if (!string.IsNullOrWhiteSpace(utilisateur))
            query = query.Where(a => a.Utilisateur == utilisateur);

        if (from.HasValue)
            query = query.Where(a => a.Date >= from.Value);

        if (to.HasValue)
            query = query.Where(a => a.Date <= to.Value);

        return await query.OrderByDescending(a => a.Date).ToListAsync();
    }
}
