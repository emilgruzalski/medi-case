using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Premedical.Authentication;
using Premedical.DataTransferObjects;
using Premedical.Services.Interfaces;

namespace Premedical.Controllers;

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

    [HttpPost]
    public async Task<ActionResult<SynchronizationResultDTO>> Publish() =>
        Ok(await _synchronizationService.SynchronizeAsync());
}
