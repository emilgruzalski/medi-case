using Microsoft.EntityFrameworkCore;
using Premedical.Models;
using Premedical.Repositories.Interfaces;

namespace Premedical.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MedicalContext _context;

    public UserRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<User?> GetByIdAsync(int id) =>
        _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public Task<User?> GetByEmailAsync(string email) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public Task<bool> EmailExistsAsync(string email) =>
        _context.Users.AnyAsync(u => u.Email == email);

    public void Add(User user) => _context.Users.Add(user);

    public void Remove(User user) => _context.Users.Remove(user);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
