using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Premedical.DataTransferObjects;
using Premedical.Services.Interfaces;

namespace Premedical.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LearningController : ControllerBase
{
    private readonly ILearningService _learningService;

    public LearningController(ILearningService learningService)
    {
        _learningService = learningService;
    }

    [HttpGet("languages")]
    public async Task<ActionResult<IEnumerable<EntityLanguageGetDTO>>> GetLanguages() =>
        Ok(await _learningService.GetLanguagesAsync());

    [HttpGet("tree")]
    public async Task<ActionResult<IEnumerable<EntityGraphObjectDTO>>> GetTree([FromQuery] int langId) =>
        Ok(await _learningService.GetTreeAsync(langId));

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<EntitySearchResultDTO>>> Search([FromQuery] string? query, [FromQuery] int langId) =>
        Ok(await _learningService.SearchAsync(query, langId));

    [HttpGet("entity/{id:guid}")]
    public async Task<ActionResult<EntityWithTranslationDTO>> GetEntity(Guid id, [FromQuery] int langId) =>
        Ok(await _learningService.GetEntityAsync(id, langId));

    [HttpGet("entity/{id:guid}/dictionary")]
    public async Task<ActionResult<IEnumerable<EntityDictionaryMainDTO>>> GetDictionary(Guid id, [FromQuery] int langId) =>
        Ok(await _learningService.GetDictionaryAsync(id, langId));

    [HttpGet("files/{id:guid}")]
    public async Task<IActionResult> GetFile(Guid id)
    {
        var download = await _learningService.DownloadFileAsync(id);
        return File(download.Content, download.ContentType, download.FileName, enableRangeProcessing: true);
    }
}
