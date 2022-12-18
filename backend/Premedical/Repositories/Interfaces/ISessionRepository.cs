using Premedical.Models;

namespace Premedical.Repositories.Interfaces;

public interface ISessionRepository
{
    Task<Session?> GetByTokenAsync(string token);
    void Add(Session session);
    void Remove(Session session);
    Task SaveChangesAsync();
}
