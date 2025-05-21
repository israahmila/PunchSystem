using PunchSystem.DTOs;
using PunchSystem.Models;

namespace PunchSystem.Contracts
{
    public interface IProduitService
    {
        Task<IEnumerable<Produit>> GetAllAsync();
        Task<Produit?> GetByIdAsync(string id);
        Task<Produit> CreateAsync(Produit produit);
        Task<bool> UpdateAsync(string id, Produit updatedProduit, string raison);
        Task<bool> SoftDeleteAsync(string id, string raison);
    }

}
