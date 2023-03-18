using Microsoft.EntityFrameworkCore;
using Premedical.Models;
using Premedical.Repositories.Interfaces;

namespace Premedical.Repositories;

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
            .Include(e => e.EntityTranslations)
            .AsSplitQuery()
            .ToListAsync();
}
