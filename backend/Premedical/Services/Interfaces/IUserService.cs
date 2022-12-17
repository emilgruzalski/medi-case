using Premedical.DataTransferObjects;

namespace Premedical.Services.Interfaces;

public interface IUserService
{
    Task<UserDTO> RegisterAsync(UserRegisterDTO dto);
    Task<UserDTO> GetByIdAsync(int id);
}
