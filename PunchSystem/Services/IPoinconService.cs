using PunchSystem.Models;

namespace PunchSystem.Services
{
    public interface IPoinconService
    {
        Task<IEnumerable<Poincon>> GetAllAsync();
        Task<Poincon?> GetByIdAsync(string id);
        Task CreateAsync(Poincon poincon);
        Task UpdateAsync(string id, Poincon poincon);
        Task SoftDeleteAsync(string id, string raison, string utilisateur);
    }

}
