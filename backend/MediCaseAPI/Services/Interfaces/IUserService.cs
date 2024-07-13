using System.Security.Claims;
using MediCaseAPI.DataTransferObjects;

namespace MediCaseAPI.Services.Interfaces;

public interface IUserService
{
    Task<UserDTO> GetCurrentAsync(ClaimsPrincipal principal);
    Task<UserDTO> UpdateCurrentAsync(ClaimsPrincipal principal, UserUpdateDTO dto);
    Task<UserDTO> GetByIdAsync(int id);
    Task<PagedResultDTO<UserDTO>> GetPageAsync(PagingParameters parameters);
    Task<UserDTO> CreateAsync(UserAdminPostDTO dto);
    Task<UserDTO> UpdateAsync(int id, UserAdminUpdateDTO dto);
    Task DeleteAsync(int id, ClaimsPrincipal principal);
}
