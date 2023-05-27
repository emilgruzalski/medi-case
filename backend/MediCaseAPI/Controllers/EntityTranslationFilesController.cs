using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCaseAPI.Authentication;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ModeratorOrAdmin)]
public class EntityTranslationFilesController : ControllerBase
{
    private readonly IEntityTranslationFileService _fileService;

    public EntityTranslationFilesController(IEntityTranslationFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet("translation/{translationId:guid}")]
    public async Task<ActionResult<IEnumerable<EntityTranslationFileDTO>>> GetTranslationFiles(Guid translationId) =>
        Ok(await _fileService.GetByTranslationAsync(translationId));

    [HttpPost]
    public async Task<ActionResult<EntityTranslationFileDTO>> UploadFile([FromForm] EntityTranslationFilePostDTO dto) =>
        Ok(await _fileService.UploadAsync(dto));

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> DownloadFile(Guid id)
    {
        var download = await _fileService.DownloadAsync(id);
        return File(download.Content, download.ContentType, download.FileName, enableRangeProcessing: true);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFile(Guid id)
    {
        await _fileService.DeleteAsync(id);
        return NoContent();
    }
}
