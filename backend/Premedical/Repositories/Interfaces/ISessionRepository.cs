using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(int id);
    Task<Session?> GetByTokenAsync(string token);
    Task<List<Session>> GetExpiredAsync(int userId, DateTime now);
    void Add(Session session);
    void Remove(Session session);
    void RemoveRange(IEnumerable<Session> sessions);
    Task SaveChangesAsync();
}
