using Premedical.DataTransferObjects;

namespace Premedical.Services.Interfaces;

public interface IEntityTranslationService
{
    Task<IEnumerable<EntityTranslationDTO>> GetByEntityAsync(Guid entityId);
    Task<EntityTranslationDTO> GetByIdAsync(Guid id);
    Task<EntityTranslationDTO> CreateAsync(EntityTranslationPostDTO dto);
    Task<EntityTranslationDTO> UpdateAsync(Guid id, EntityTranslationUpdateDTO dto);
    Task DeleteAsync(Guid id);
}
