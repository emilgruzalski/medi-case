using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    void Add(User user);
    void Remove(User user);
    Task SaveChangesAsync();
}
