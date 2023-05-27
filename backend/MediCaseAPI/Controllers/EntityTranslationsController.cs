using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCaseAPI.Authentication;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ModeratorOrAdmin)]
public class EntityTranslationsController : ControllerBase
{
    private readonly IEntityTranslationService _translationService;

    public EntityTranslationsController(IEntityTranslationService translationService)
    {
        _translationService = translationService;
    }

    [HttpGet("entity/{entityId:guid}")]
    public async Task<ActionResult<IEnumerable<EntityTranslationDTO>>> GetEntityTranslations(Guid entityId) =>
        Ok(await _translationService.GetByEntityAsync(entityId));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EntityTranslationDTO>> GetTranslation(Guid id) =>
        Ok(await _translationService.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<EntityTranslationDTO>> CreateTranslation(EntityTranslationPostDTO dto)
    {
        var translation = await _translationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetTranslation), new { id = translation.Id }, translation);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EntityTranslationDTO>> UpdateTranslation(Guid id, EntityTranslationUpdateDTO dto) =>
        Ok(await _translationService.UpdateAsync(id, dto));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTranslation(Guid id)
    {
        await _translationService.DeleteAsync(id);
        return NoContent();
    }
}
