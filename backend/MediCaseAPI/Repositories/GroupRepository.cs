using Microsoft.EntityFrameworkCore;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories.Interfaces;

namespace MediCaseAPI.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly MedicalContext _context;

    public GroupRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<List<Group>> GetPageAsync(string? search, int skip, int take) =>
        Filter(search)
            .AsNoTracking()
            .Include(g => g.Users)
            .OrderBy(g => g.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

    public Task<int> CountAsync(string? search) => Filter(search).CountAsync();

    public Task<Group?> GetByIdAsync(int id) =>
        _context.Groups.Include(g => g.Users).ThenInclude(u => u.Groups).FirstOrDefaultAsync(g => g.Id == id);

    public Task<List<Group>> GetByIdsAsync(IReadOnlyCollection<int> ids) =>
        _context.Groups.Where(g => ids.Contains(g.Id)).ToListAsync();

    public Task<bool> NameExistsAsync(string name, int? exceptId = null) =>
        _context.Groups.AnyAsync(g => g.Name == name && g.Id != exceptId);

    public void Add(Group group) => _context.Groups.Add(group);

    public void Remove(Group group) => _context.Groups.Remove(group);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();

    private IQueryable<Group> Filter(string? search) =>
        search is null ? _context.Groups : _context.Groups.Where(g => g.Name.Contains(search));
}
