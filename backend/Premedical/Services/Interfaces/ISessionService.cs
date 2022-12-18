using Premedical.DataTransferObjects;

namespace Premedical.Services.Interfaces;

public interface ISessionService
{
    Task<SessionDTO> LoginAsync(LoginDTO dto);
}
