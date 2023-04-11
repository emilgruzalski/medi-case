using Microsoft.EntityFrameworkCore;
using Premedical.Models;
using Premedical.Repositories.Interfaces;

namespace Premedical.Repositories;

public class EntityTranslationFilesRepository : IEntityTranslationFilesRepository
{
    private readonly MedicalContext _context;

    public EntityTranslationFilesRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<EntityTranslationFile?> GetByIdAsync(Guid id) =>
        _context.EntityTranslationFiles
            .Include(f => f.Translation).ThenInclude(t => t.Entity)
            .FirstOrDefaultAsync(f => f.Id == id);

    public Task<List<EntityTranslationFile>> GetByTranslationAsync(Guid translationId) =>
        _context.EntityTranslationFiles
            .AsNoTracking()
            .Where(f => f.TranslationId == translationId)
            .OrderBy(f => f.UploadedAt)
            .ToListAsync();

    public void Add(EntityTranslationFile file) => _context.EntityTranslationFiles.Add(file);

    public void Remove(EntityTranslationFile file) => _context.EntityTranslationFiles.Remove(file);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
