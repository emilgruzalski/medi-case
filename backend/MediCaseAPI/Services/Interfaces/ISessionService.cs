using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Models;

namespace MediCaseAPI.Services.Interfaces;

public interface ISessionService
{
    Task<SessionDTO> LoginAsync(LoginDTO dto);
    Task<Session?> GetActiveSessionAsync(string token);
    Task LogoutAsync(int sessionId);
}
