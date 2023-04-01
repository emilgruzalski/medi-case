using Premedical.DataTransferObjects;

namespace Premedical.Services.Interfaces;

public interface IEntityService
{
    Task<EntityDTO> GetByIdAsync(Guid id);
    Task<EntityDTO> CreateAsync(EntityPostDTO dto);
    Task<EntityWithTranslationDTO> GetWithChildrenAsync(Guid id, int langId);
}
