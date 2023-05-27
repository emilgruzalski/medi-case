using MediCaseAPI.Models;

namespace MediCaseAPI.Repositories.Interfaces;

public interface IEntityTypeRepository
{
    Task<List<EntityType>> GetAllAsync();
    Task<EntityType?> GetByIdAsync(int id);
}
