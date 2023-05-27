using MediCaseAPI.DataTransferObjects;

namespace MediCaseAPI.Services.Interfaces;

public interface IEntityDictionaryService
{
    Task<IEnumerable<EntityDictionaryMainDTO>> GetByEntityAsync(Guid entityId, int? langId);
    Task<EntityDictionaryMainDTO> GetByIdAsync(Guid id);
    Task<EntityDictionaryMainDTO> CreateAsync(EntityDictionaryPostDTO dto);
    Task<EntityDictionaryMainDTO> UpdateAsync(Guid id, EntityDictionaryUpdateDTO dto);
    Task DeleteAsync(Guid id);
}
