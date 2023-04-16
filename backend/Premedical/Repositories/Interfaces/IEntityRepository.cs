using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface IEntityRepository
{
    Task<Entity?> GetByIdAsync(Guid id);
    Task<Entity?> GetWithDetailsAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<List<Entity>> GetByIdsAsync(IReadOnlyCollection<Guid> ids);
    void Add(Entity entity);
    void Remove(Entity entity);
    void RemoveRange(IEnumerable<Entity> entities);
    Task SaveChangesAsync();
}
