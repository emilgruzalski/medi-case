using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Premedical.Authentication;
using Premedical.DataTransferObjects;
using Premedical.Services.Interfaces;

namespace Premedical.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ModeratorOrAdmin)]
public class AiController : ControllerBase
{
    private readonly IAiContentService _aiContentService;

    public AiController(IAiContentService aiContentService)
    {
        _aiContentService = aiContentService;
    }

    [HttpPost("translate")]
    public async Task<ActionResult<EntityTranslationDTO>> Translate(AiTranslationPostDTO dto) =>
        Ok(await _aiContentService.TranslateAsync(dto));
}
