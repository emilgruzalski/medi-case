using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Premedical.Authentication;
using Premedical.DataTransferObjects;
using Premedical.Extensions;
using Premedical.Services.Interfaces;

namespace Premedical.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(UserRegisterDTO dto)
    {
        var user = await _userService.RegisterAsync(dto);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDTO>> GetCurrentUser() =>
        Ok(await _userService.GetByIdAsync(User.GetUserId()));

    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult<UserDTO>> UpdateCurrentUser(UserUpdateDTO dto) =>
        Ok(await _userService.UpdateProfileAsync(User.GetUserId(), dto));

    [Authorize]
    [HttpPut("me/password")]
    public async Task<IActionResult> ChangePassword(UserPasswordDTO dto)
    {
        await _userService.ChangePasswordAsync(User.GetUserId(), dto);
        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpGet]
    public async Task<ActionResult<PagedResultDTO<UserDTO>>> GetUsers([FromQuery] PagingParameters parameters) =>
        Ok(await _userService.GetPageAsync(parameters));

    [Authorize(Roles = Roles.Admin)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDTO>> GetUser(int id) =>
        Ok(await _userService.GetByIdAsync(id));

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<ActionResult<UserDTO>> CreateUser(UserAdminPostDTO dto)
    {
        var user = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDTO>> UpdateUser(int id, UserAdminUpdateDTO dto) =>
        Ok(await _userService.UpdateAsync(id, dto));

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteAsync(id, User.GetUserId());
        return NoContent();
    }
}
