using PunchSystem.Models;

namespace PunchSystem.Contracts
{
    public interface IPoinconService
    {
        Task<IEnumerable<Poincon>> GetAllAsync();
        Task<Poincon?> GetByIdAsync(string id);
        Task<Poincon> CreateAsync(Poincon poincon);
        Task<bool> UpdateAsync(string id, Poincon poincon, string raison);
        Task<bool> SoftDeleteAsync(string id, string raison);
    }


}
