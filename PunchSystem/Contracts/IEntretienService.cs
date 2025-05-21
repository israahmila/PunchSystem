using PunchSystem.Models;

namespace PunchSystem.Contracts
{
    public interface IEntretienService
    {
        Task<IEnumerable<Entretien>> GetAllAsync();
        Task<Entretien?> GetByIdAsync(string id);
        Task<Entretien> CreateAsync(Entretien entretien);
        Task<bool> UpdateAsync(string id, Entretien entretien, string raison);
        Task<bool> DeleteAsync(string id, string raison);
    }


}
