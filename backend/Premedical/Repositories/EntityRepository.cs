using Microsoft.EntityFrameworkCore;
using Premedical.Models;
using Premedical.Repositories.Interfaces;

namespace Premedical.Repositories;

public class EntityRepository : IEntityRepository
{
    private readonly MedicalContext _context;

    public EntityRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<Entity?> GetByIdAsync(Guid id) =>
        _context.Entities.FirstOrDefaultAsync(e => e.Id == id);

    public Task<Entity?> GetWithDetailsAsync(Guid id) =>
        _context.Entities
            .Include(e => e.Type)
            .Include(e => e.ParentLink)
            .Include(e => e.EntityTranslations).ThenInclude(t => t.Lang)
            .Include(e => e.EntityTranslations).ThenInclude(t => t.EntityTranslationFiles)
            .AsSplitQuery()
            .FirstOrDefaultAsync(e => e.Id == id);

    public Task<bool> ExistsAsync(Guid id) =>
        _context.Entities.AnyAsync(e => e.Id == id);

    public void Add(Entity entity) => _context.Entities.Add(entity);

    public void Remove(Entity entity) => _context.Entities.Remove(entity);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
