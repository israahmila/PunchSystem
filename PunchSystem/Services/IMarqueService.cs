using PunchSystem.Models;

namespace PunchSystem.Services
{
    public interface IMarqueService
    {
        Task<IEnumerable<Marque>> GetAllAsync();
        Task<Marque?> GetByIdAsync(string id);
        Task CreateAsync(Marque marque);
        Task UpdateAsync(string id, Marque marque);
        Task SoftDeleteAsync(string id, string raison, string utilisateur);
    }

}
