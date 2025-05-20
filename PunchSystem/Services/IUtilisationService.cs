using PunchSystem.DTOs;
using PunchSystem.Models;

namespace PunchSystem.Services
{
    public interface IUtilisationService
    {
        Task<IEnumerable<Utilisation>> GetAllAsync();
        Task<Utilisation?> GetByIdAsync(string id);
        Task<Utilisation> CreateAsync(CreateUtilisationDto dto);

        Task<bool> UpdateAsync(string id, UpdateUtilisationDto dto);
        Task DeleteAsync(string id, string raison, string utilisateur);
    }

}
