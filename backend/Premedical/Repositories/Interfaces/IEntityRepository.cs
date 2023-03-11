using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface IEntityRepository
{
    Task<Entity?> GetByIdAsync(Guid id);
    Task<Entity?> GetWithDetailsAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    void Add(Entity entity);
    void Remove(Entity entity);
    Task SaveChangesAsync();
}
