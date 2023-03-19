using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface IEntityDictionaryRepository
{
    Task<EntityDictionary?> GetByIdAsync(Guid id);
    Task<List<EntityDictionary>> GetByEntityAsync(Guid entityId, int? langId);
    void Add(EntityDictionary entry);
    void Remove(EntityDictionary entry);
    Task SaveChangesAsync();
}
