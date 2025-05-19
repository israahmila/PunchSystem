using PunchSystem.DTOs;

namespace PunchSystem.Services
{
    public interface IUtilisationService
    {
        Task<IEnumerable<UtilisationDto>> GetAllAsync();
        Task<UtilisationDto> CreateAsync(CreateUtilisationDto dto);
        Task<UtilisationDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateUtilisationDto dto);
        Task<bool> DeleteAsync(int id);


    }
}
