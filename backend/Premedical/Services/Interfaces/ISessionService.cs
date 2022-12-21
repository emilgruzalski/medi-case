using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.Services.Interfaces;

public interface ISessionService
{
    Task<SessionDTO> LoginAsync(LoginDTO dto);
    Task<Session?> GetActiveSessionAsync(string token);
    Task LogoutAsync(int sessionId);
}
