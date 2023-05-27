using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCaseAPI.Authentication;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Controllers;

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

    [HttpPost("speech/{translationId:guid}")]
    public async Task<ActionResult<EntityTranslationFileDTO>> GenerateSpeech(Guid translationId) =>
        Ok(await _aiContentService.GenerateSpeechAsync(translationId));
}
