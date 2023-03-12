using Microsoft.EntityFrameworkCore;
using Premedical.Models;
using Premedical.Repositories.Interfaces;

namespace Premedical.Repositories;

public class EntityTranslationsRepository : IEntityTranslationsRepository
{
    private readonly MedicalContext _context;

    public EntityTranslationsRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<EntityTranslation?> GetByIdAsync(Guid id) =>
        _context.EntityTranslations
            .Include(t => t.Lang)
            .Include(t => t.Entity)
            .FirstOrDefaultAsync(t => t.Id == id);

    public Task<List<EntityTranslation>> GetByEntityAsync(Guid entityId) =>
        _context.EntityTranslations
            .Include(t => t.Lang)
            .Where(t => t.EntityId == entityId)
            .OrderBy(t => t.LangId)
            .ToListAsync();

    public Task<bool> ExistsAsync(Guid entityId, int langId) =>
        _context.EntityTranslations.AnyAsync(t => t.EntityId == entityId && t.LangId == langId);

    public void Add(EntityTranslation translation) => _context.EntityTranslations.Add(translation);

    public void Remove(EntityTranslation translation) => _context.EntityTranslations.Remove(translation);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
