using Microsoft.EntityFrameworkCore;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories.Interfaces;

namespace MediCaseAPI.Repositories;

public class EntityDictionaryRepository : IEntityDictionaryRepository
{
    private readonly MedicalContext _context;

    public EntityDictionaryRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<EntityDictionary?> GetByIdAsync(Guid id) =>
        _context.EntityDictionaries
            .Include(d => d.Entity)
            .FirstOrDefaultAsync(d => d.Id == id);

    public Task<List<EntityDictionary>> GetByEntityAsync(Guid entityId, int? langId) =>
        _context.EntityDictionaries
            .AsNoTracking()
            .Where(d => d.EntityId == entityId && (langId == null || d.LangId == langId))
            .OrderBy(d => d.Term)
            .ToListAsync();

    public void Add(EntityDictionary entry) => _context.EntityDictionaries.Add(entry);

    public void Remove(EntityDictionary entry) => _context.EntityDictionaries.Remove(entry);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
