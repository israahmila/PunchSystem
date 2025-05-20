using PunchSystem.Models;

namespace PunchSystem.Services
{
    public interface IProduitService
    {
        Task<IEnumerable<Produit>> GetAllAsync();
        Task<Produit?> GetByIdAsync(string id);
        Task CreateAsync(Produit produit);
        Task UpdateAsync(string id, Produit produit);
        Task SoftDeleteAsync(string id, string raison);
    }
}

