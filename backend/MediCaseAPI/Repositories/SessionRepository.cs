using Microsoft.EntityFrameworkCore;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories.Interfaces;

namespace MediCaseAPI.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly MedicalContext _context;

    public SessionRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<Session?> GetByIdAsync(int id) =>
        _context.Sessions.FirstOrDefaultAsync(s => s.Id == id);

    public Task<Session?> GetByTokenAsync(string token) =>
        _context.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Token == token);

    public Task<List<Session>> GetExpiredAsync(int userId, DateTime now) =>
        _context.Sessions
            .Where(s => s.UserId == userId && s.ExpiresAt <= now)
            .ToListAsync();

    public void Add(Session session) => _context.Sessions.Add(session);

    public void Remove(Session session) => _context.Sessions.Remove(session);

    public void RemoveRange(IEnumerable<Session> sessions) => _context.Sessions.RemoveRange(sessions);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
