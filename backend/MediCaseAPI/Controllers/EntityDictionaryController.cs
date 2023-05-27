using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCaseAPI.Authentication;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ModeratorOrAdmin)]
public class EntityDictionaryController : ControllerBase
{
    private readonly IEntityDictionaryService _dictionaryService;

    public EntityDictionaryController(IEntityDictionaryService dictionaryService)
    {
        _dictionaryService = dictionaryService;
    }

    [HttpGet("entity/{entityId:guid}")]
    public async Task<ActionResult<IEnumerable<EntityDictionaryMainDTO>>> GetEntityDictionary(
        Guid entityId, [FromQuery] int? langId) =>
        Ok(await _dictionaryService.GetByEntityAsync(entityId, langId));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EntityDictionaryMainDTO>> GetEntry(Guid id) =>
        Ok(await _dictionaryService.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<EntityDictionaryMainDTO>> CreateEntry(EntityDictionaryPostDTO dto)
    {
        var entry = await _dictionaryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetEntry), new { id = entry.Id }, entry);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EntityDictionaryMainDTO>> UpdateEntry(Guid id, EntityDictionaryUpdateDTO dto) =>
        Ok(await _dictionaryService.UpdateAsync(id, dto));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEntry(Guid id)
    {
        await _dictionaryService.DeleteAsync(id);
        return NoContent();
    }
}
