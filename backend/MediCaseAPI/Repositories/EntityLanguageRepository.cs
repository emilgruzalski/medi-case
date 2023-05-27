using Microsoft.EntityFrameworkCore;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories.Interfaces;

namespace MediCaseAPI.Repositories;

public class EntityLanguageRepository : IEntityLanguageRepository
{
    private readonly MedicalContext _context;

    public EntityLanguageRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<List<EntityLanguage>> GetAllAsync() =>
        _context.EntityLanguages.AsNoTracking().OrderBy(l => l.Id).ToListAsync();

    public Task<EntityLanguage?> GetByIdAsync(int id) =>
        _context.EntityLanguages.FirstOrDefaultAsync(l => l.Id == id);

    public Task<bool> LangValueExistsAsync(string langValue, int? exceptId = null) =>
        _context.EntityLanguages.AnyAsync(l => l.LangValue == langValue && l.Id != exceptId);

    public async Task<int> GetMaxIdAsync() =>
        await _context.EntityLanguages.MaxAsync(l => (int?)l.Id) ?? 0;

    public async Task<bool> IsUsedAsync(int id) =>
        await _context.EntityTranslations.AnyAsync(t => t.LangId == id) ||
        await _context.EntityDictionaries.AnyAsync(d => d.LangId == id);

    public void Add(EntityLanguage language) => _context.EntityLanguages.Add(language);

    public void Remove(EntityLanguage language) => _context.EntityLanguages.Remove(language);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
