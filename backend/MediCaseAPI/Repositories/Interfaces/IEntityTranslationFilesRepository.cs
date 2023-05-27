using MediCaseAPI.Models;

namespace MediCaseAPI.Repositories.Interfaces;

public interface IEntityTranslationFilesRepository
{
    Task<EntityTranslationFile?> GetByIdAsync(Guid id);
    Task<List<EntityTranslationFile>> GetByTranslationAsync(Guid translationId);
    void Add(EntityTranslationFile file);
    void Remove(EntityTranslationFile file);
    Task SaveChangesAsync();
}
