using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface IEntityTypeRepository
{
    Task<List<EntityType>> GetAllAsync();
    Task<EntityType?> GetByIdAsync(int id);
}
