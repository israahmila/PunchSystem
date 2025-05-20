using PunchSystem.Models;

namespace PunchSystem.Services
{
    public interface IEntretienService
    {
        Task<IEnumerable<Entretien>> GetAllAsync();
        Task<Entretien?> GetByIdAsync(string id);
        Task CreateAsync(Entretien entretien);
        Task UpdateAsync(string id, Entretien entretien);
        Task DeleteAsync(string id, string raison, string utilisateur);
    }

}
