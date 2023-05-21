using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetPageAsync(string? search, int skip, int take);
    Task<int> CountAsync(string? search);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    void Add(User user);
    void Remove(User user);
    Task SaveChangesAsync();
}
