using Microsoft.EntityFrameworkCore;
using Premedical.Models;
using Premedical.Repositories.Interfaces;

namespace Premedical.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly MedicalContext _context;

    public SessionRepository(MedicalContext context)
    {
        _context = context;
    }

    public Task<Session?> GetByTokenAsync(string token) =>
        _context.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Token == token);

    public void Add(Session session) => _context.Sessions.Add(session);

    public void Remove(Session session) => _context.Sessions.Remove(session);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
