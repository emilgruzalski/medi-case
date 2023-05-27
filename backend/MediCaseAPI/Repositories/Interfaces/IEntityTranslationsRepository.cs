using MediCaseAPI.Models;

namespace MediCaseAPI.Repositories.Interfaces;

public interface IEntityTranslationsRepository
{
    Task<EntityTranslation?> GetByIdAsync(Guid id);
    Task<List<EntityTranslation>> GetByEntityAsync(Guid entityId);
    Task<bool> ExistsAsync(Guid entityId, int langId);
    void Add(EntityTranslation translation);
    void Remove(EntityTranslation translation);
    Task SaveChangesAsync();
}
