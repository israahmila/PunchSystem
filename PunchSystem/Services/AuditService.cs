using PunchSystem.Contracts;
using PunchSystem.Data;
using PunchSystem.Models;

namespace PunchSystem.Services
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;

        public AuditService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string module, string action, string reference, string utilisateur, string raison)
        {
            var entry = new AuditTrail
            {
                Id = Helpers.IdGenerator.New("AUD"),
                Module = module,
                Action = action,
                ReferenceObjet = reference,
                Date = DateTime.UtcNow,
                Utilisateur = utilisateur,
                Raison = raison
            };

            _context.AuditTrails.Add(entry);
            await _context.SaveChangesAsync();
        }
    }
}
