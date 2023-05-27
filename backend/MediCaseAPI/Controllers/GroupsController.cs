using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCaseAPI.Authentication;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDTO<GroupDTO>>> GetGroups([FromQuery] PagingParameters parameters) =>
        Ok(await _groupService.GetPageAsync(parameters));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GroupDTO>> GetGroup(int id) =>
        Ok(await _groupService.GetByIdAsync(id));

    [HttpGet("{id:int}/users")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetGroupUsers(int id) =>
        Ok(await _groupService.GetUsersAsync(id));

    [HttpPost]
    public async Task<ActionResult<GroupDTO>> CreateGroup(GroupPostDTO dto)
    {
        var group = await _groupService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GroupDTO>> UpdateGroup(int id, GroupPostDTO dto) =>
        Ok(await _groupService.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        await _groupService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id:int}/users/{userId:int}")]
    public async Task<IActionResult> AddUser(int id, int userId)
    {
        await _groupService.AddUserAsync(id, userId);
        return NoContent();
    }

    [HttpDelete("{id:int}/users/{userId:int}")]
    public async Task<IActionResult> RemoveUser(int id, int userId)
    {
        await _groupService.RemoveUserAsync(id, userId);
        return NoContent();
    }
}
