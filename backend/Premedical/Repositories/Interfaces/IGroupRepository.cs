using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface IGroupRepository
{
    Task<List<Group>> GetPageAsync(string? search, int skip, int take);
    Task<int> CountAsync(string? search);
    Task<Group?> GetByIdAsync(int id);
    Task<List<Group>> GetByIdsAsync(IReadOnlyCollection<int> ids);
    Task<bool> NameExistsAsync(string name, int? exceptId = null);
    void Add(Group group);
    void Remove(Group group);
    Task SaveChangesAsync();
}
