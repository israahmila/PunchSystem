using PunchSystem.Models;

namespace PunchSystem.Services
{
    public interface IFournisseurService
    {
        Task<IEnumerable<Fournisseur>> GetAllAsync();
        Task<Fournisseur?> GetByIdAsync(string id);
        Task CreateAsync(Fournisseur fournisseur);
        Task UpdateAsync(string id, Fournisseur fournisseur);
        Task SoftDeleteAsync(string id, string raison, string utilisateur);
    }

}
