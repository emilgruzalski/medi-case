using Premedical.DataTransferObjects;

namespace Premedical.Services.Interfaces;

public interface IEntityLanguageService
{
    Task<IEnumerable<EntityLanguageGetDTO>> GetAllAsync();
    Task<EntityLanguageGetDTO> GetByIdAsync(int id);
    Task<EntityLanguageGetDTO> CreateAsync(EntityLanguagePostDTO dto);
    Task<EntityLanguageGetDTO> UpdateAsync(int id, EntityLanguageUpdateDTO dto);
    Task DeleteAsync(int id);
}
