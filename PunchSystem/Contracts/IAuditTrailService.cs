using PunchSystem.Models;

namespace PunchSystem.Contracts
{
    public interface IAuditTrailService
    {
        Task<IEnumerable<AuditTrail>> GetAllAsync(string? module = null, string? utilisateur = null, DateTime? from = null, DateTime? to = null);
    }

}
