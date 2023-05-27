using MediCaseAPI.DataTransferObjects;

namespace MediCaseAPI.Services.Interfaces;

public interface IEntityService
{
    Task<EntityDTO> GetByIdAsync(Guid id);
    Task<EntityDTO> CreateAsync(EntityPostDTO dto);
    Task<EntityWithTranslationDTO> GetWithChildrenAsync(Guid id, int langId);
    Task<EntityDTO> UpdateAsync(Guid id, EntityUpdateDTO dto);
    Task<EntityDTO> MoveAsync(Guid id, EntityMoveDTO dto);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<EntitySearchResultDTO>> SearchAsync(string? query, int langId);
}
