using Microsoft.EntityFrameworkCore;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories.Interfaces;

namespace MediCaseAPI.Repositories;

public class EntityGraphDataRepository : IEntityGraphDataRepository
{
    private readonly MedicalContext _context;

    public EntityGraphDataRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<List<EntitiesGraphDatum>> GetAllAsync() =>
        _context.EntitiesGraphData.AsNoTracking().ToListAsync();

    public Task<List<Entity>> GetEntitiesForTreeAsync() =>
        _context.Entities
            .AsNoTracking()
            .Include(e => e.ParentLink)
            .Include(e => e.EntityTranslations).ThenInclude(t => t.EntityTranslationFiles)
            .AsSplitQuery()
            .ToListAsync();
}
