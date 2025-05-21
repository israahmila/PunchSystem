using PunchSystem.Models;

namespace PunchSystem.Contracts
{
    public interface IFournisseurService
    {
        Task<IEnumerable<Fournisseur>> GetAllAsync();
        Task<Fournisseur?> GetByIdAsync(string id);
        Task<Fournisseur> CreateAsync(Fournisseur fournisseur);
        Task<bool> UpdateAsync(string id, Fournisseur fournisseur, string raison);
        Task<bool> SoftDeleteAsync(string id, string raison);
    }


}
