using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Exceptions;
using Premedical.Models;
using Premedical.Repositories.Interfaces;
using Premedical.Services.Interfaces;

namespace Premedical.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GroupService(IGroupRepository groupRepository, IUserRepository userRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDTO<GroupDTO>> GetPageAsync(PagingParameters parameters)
    {
        var search = parameters.NormalizedSearch;
        var groups = await _groupRepository.GetPageAsync(search, parameters.Skip, parameters.PageSize);
        var total = await _groupRepository.CountAsync(search);
        return new PagedResultDTO<GroupDTO>(_mapper.Map<IEnumerable<GroupDTO>>(groups), total, parameters);
    }

    public async Task<GroupDTO> GetByIdAsync(int id) =>
        _mapper.Map<GroupDTO>(await GetGroupOrThrowAsync(id));

    public async Task<IEnumerable<UserDTO>> GetUsersAsync(int id)
    {
        var group = await GetGroupOrThrowAsync(id);
        return _mapper.Map<IEnumerable<UserDTO>>(group.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName));
    }

    public async Task<GroupDTO> CreateAsync(GroupPostDTO dto)
    {
        var group = _mapper.Map<Group>(dto);
        if (await _groupRepository.NameExistsAsync(group.Name))
            throw new ConflictException($"Group '{group.Name}' already exists.");

        group.CreatedAt = DateTime.UtcNow;
        _groupRepository.Add(group);
        await _groupRepository.SaveChangesAsync();

        return _mapper.Map<GroupDTO>(group);
    }

    public async Task<GroupDTO> UpdateAsync(int id, GroupPostDTO dto)
    {
        var group = await GetGroupOrThrowAsync(id);
        _mapper.Map(dto, group);

        if (await _groupRepository.NameExistsAsync(group.Name, id))
            throw new ConflictException($"Group '{group.Name}' already exists.");

        await _groupRepository.SaveChangesAsync();
        return _mapper.Map<GroupDTO>(group);
    }

    public async Task DeleteAsync(int id)
    {
        var group = await GetGroupOrThrowAsync(id);
        _groupRepository.Remove(group);
        await _groupRepository.SaveChangesAsync();
    }

    public async Task AddUserAsync(int id, int userId)
    {
        var group = await GetGroupOrThrowAsync(id);
        var user = await GetUserOrThrowAsync(userId);

        if (group.Users.All(u => u.Id != user.Id))
            group.Users.Add(user);
        await _groupRepository.SaveChangesAsync();
    }

    public async Task RemoveUserAsync(int id, int userId)
    {
        var group = await GetGroupOrThrowAsync(id);
        var user = group.Users.FirstOrDefault(u => u.Id == userId)
            ?? throw new NotFoundException($"User with id {userId} is not a member of this group.");

        group.Users.Remove(user);
        await _groupRepository.SaveChangesAsync();
    }

    private async Task<Group> GetGroupOrThrowAsync(int id) =>
        await _groupRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"Group with id {id} was not found.");

    private async Task<User> GetUserOrThrowAsync(int id) =>
        await _userRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"User with id {id} was not found.");
}
