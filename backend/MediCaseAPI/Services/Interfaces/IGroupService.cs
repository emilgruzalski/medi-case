using MediCaseAPI.DataTransferObjects;

namespace MediCaseAPI.Services.Interfaces;

public interface IGroupService
{
    Task<PagedResultDTO<GroupDTO>> GetPageAsync(PagingParameters parameters);
    Task<GroupDTO> GetByIdAsync(int id);
    Task<IEnumerable<UserDTO>> GetUsersAsync(int id);
    Task<GroupDTO> CreateAsync(GroupPostDTO dto);
    Task<GroupDTO> UpdateAsync(int id, GroupPostDTO dto);
    Task DeleteAsync(int id);
    Task AddUserAsync(int id, int userId);
    Task RemoveUserAsync(int id, int userId);
}
