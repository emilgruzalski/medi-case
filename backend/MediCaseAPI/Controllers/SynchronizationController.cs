using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCaseAPI.Authentication;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class SynchronizationController : ControllerBase
{
    private readonly ISynchronizationService _synchronizationService;

    public SynchronizationController(ISynchronizationService synchronizationService)
    {
        _synchronizationService = synchronizationService;
    }

    [HttpGet("changes")]
    public async Task<ActionResult<PendingChangesDTO>> GetPendingChanges([FromQuery] int langId) =>
        Ok(await _synchronizationService.GetPendingChangesAsync(langId));

    [HttpPost]
    public async Task<ActionResult<SynchronizationResultDTO>> Publish() =>
        Ok(await _synchronizationService.SynchronizeAsync());
}
