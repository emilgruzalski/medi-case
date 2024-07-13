using MediCaseAPI.Models;

namespace MediCaseAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetPageAsync(string? search, int skip, int take);
    Task<int> CountAsync(string? search);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByCognitoUsernameAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    void Add(User user);
    void Remove(User user);
    Task SaveChangesAsync();
}
