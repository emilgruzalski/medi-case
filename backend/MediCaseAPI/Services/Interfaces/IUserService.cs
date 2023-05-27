using MediCaseAPI.DataTransferObjects;

namespace MediCaseAPI.Services.Interfaces;

public interface IUserService
{
    Task<UserDTO> RegisterAsync(UserRegisterDTO dto);
    Task<UserDTO> GetByIdAsync(int id);
    Task<PagedResultDTO<UserDTO>> GetPageAsync(PagingParameters parameters);
    Task<UserDTO> CreateAsync(UserAdminPostDTO dto);
    Task<UserDTO> UpdateAsync(int id, UserAdminUpdateDTO dto);
    Task DeleteAsync(int id, int currentUserId);
    Task<UserDTO> UpdateProfileAsync(int id, UserUpdateDTO dto);
    Task ChangePasswordAsync(int id, UserPasswordDTO dto);
}
