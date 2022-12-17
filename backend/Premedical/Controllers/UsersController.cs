using Microsoft.AspNetCore.Mvc;
using Premedical.DataTransferObjects;
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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDTO>> GetUser(int id) =>
        Ok(await _userService.GetByIdAsync(id));
}
