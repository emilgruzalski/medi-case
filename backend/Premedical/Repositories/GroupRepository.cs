using Microsoft.EntityFrameworkCore;
using Premedical.Models;
using Premedical.Repositories.Interfaces;

namespace Premedical.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly MedicalContext _context;

    public GroupRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<List<Group>> GetAllAsync() =>
        _context.Groups.AsNoTracking().Include(g => g.Users).OrderBy(g => g.Name).ToListAsync();

    public Task<Group?> GetByIdAsync(int id) =>
        _context.Groups.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == id);

    public Task<List<Group>> GetByIdsAsync(IReadOnlyCollection<int> ids) =>
        _context.Groups.Where(g => ids.Contains(g.Id)).ToListAsync();

    public Task<bool> NameExistsAsync(string name, int? exceptId = null) =>
        _context.Groups.AnyAsync(g => g.Name == name && g.Id != exceptId);

    public void Add(Group group) => _context.Groups.Add(group);

    public void Remove(Group group) => _context.Groups.Remove(group);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
