using MediCaseAPI.Models;

namespace MediCaseAPI.Repositories.Interfaces;

public interface IEntityGraphDataRepository
{
    Task<List<EntitiesGraphDatum>> GetAllAsync();
    Task<List<Entity>> GetEntitiesForTreeAsync();
}
