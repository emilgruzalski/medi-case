using Microsoft.EntityFrameworkCore;
using Premedical.Models;
using Premedical.Repositories.Interfaces;

namespace Premedical.Repositories;

public class EntityTypeRepository : IEntityTypeRepository
{
    private readonly MedicalContext _context;

    public EntityTypeRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<List<EntityType>> GetAllAsync() =>
        _context.EntityTypes.AsNoTracking().OrderBy(t => t.Id).ToListAsync();

    public Task<EntityType?> GetByIdAsync(int id) =>
        _context.EntityTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
}
