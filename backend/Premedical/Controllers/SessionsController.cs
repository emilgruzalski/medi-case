using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Premedical.DataTransferObjects;
using Premedical.Extensions;
using Premedical.Services.Interfaces;

namespace Premedical.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<SessionDTO>> Login(LoginDTO dto) =>
        Ok(await _sessionService.LoginAsync(dto));

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _sessionService.LogoutAsync(User.GetSessionId());
        return NoContent();
    }
}
