using MediCaseAPI.Models;

namespace MediCaseAPI.Repositories.Interfaces;

public interface IEntityRepository
{
    Task<Entity?> GetByIdAsync(Guid id);
    Task<Entity?> GetWithDetailsAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<List<Entity>> GetByIdsAsync(IReadOnlyCollection<Guid> ids);
    Task<Entity?> GetWithParentLinkAsync(Guid id);
    Task<int> GetMaxChildOrderAsync(Guid? parentId);
    void Add(Entity entity);
    void Remove(Entity entity);
    void RemoveRange(IEnumerable<Entity> entities);
    Task SaveChangesAsync();
}
