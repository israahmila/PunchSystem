using PunchSystem.Models;

namespace PunchSystem.Contracts
{
    public interface IMarqueService
    {
        Task<IEnumerable<Marque>> GetAllAsync();
        Task<Marque?> GetByIdAsync(string id);
        Task<Marque> CreateAsync(Marque marque);
        Task<bool> UpdateAsync(string id, Marque updated, string raison);
        Task<bool> SoftDeleteAsync(string id, string raison);
    }


}
