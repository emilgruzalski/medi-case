using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface IEntityLanguageRepository
{
    Task<List<EntityLanguage>> GetAllAsync();
    Task<EntityLanguage?> GetByIdAsync(int id);
    Task<bool> LangValueExistsAsync(string langValue, int? exceptId = null);
    Task<int> GetMaxIdAsync();
    Task<bool> IsUsedAsync(int id);
    void Add(EntityLanguage language);
    void Remove(EntityLanguage language);
    Task SaveChangesAsync();
}
