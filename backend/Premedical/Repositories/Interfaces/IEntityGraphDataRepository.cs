using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface IEntityGraphDataRepository
{
    Task<List<EntitiesGraphDatum>> GetAllAsync();
    Task<List<Entity>> GetEntitiesForTreeAsync();
}
