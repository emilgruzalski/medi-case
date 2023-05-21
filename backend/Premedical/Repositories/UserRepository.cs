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

    public Task<List<User>> GetPageAsync(string? search, int skip, int take) =>
        Filter(search)
            .AsNoTracking()
            .Include(u => u.Groups)
            .OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ThenBy(u => u.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

    public Task<int> CountAsync(string? search) => Filter(search).CountAsync();

    public Task<User?> GetByIdAsync(int id) =>
        _context.Users.Include(u => u.Groups).FirstOrDefaultAsync(u => u.Id == id);

    public Task<User?> GetByEmailAsync(string email) =>
        _context.Users.Include(u => u.Groups).FirstOrDefaultAsync(u => u.Email == email);

    public Task<bool> EmailExistsAsync(string email) =>
        _context.Users.AnyAsync(u => u.Email == email);

    public void Add(User user) => _context.Users.Add(user);

    public void Remove(User user) => _context.Users.Remove(user);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();

    private IQueryable<User> Filter(string? search) =>
        search is null
            ? _context.Users
            : _context.Users.Where(u =>
                u.Email.Contains(search) || u.FirstName.Contains(search) || u.LastName.Contains(search));
}
