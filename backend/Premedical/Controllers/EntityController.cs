using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Premedical.Authentication;
using Premedical.DataTransferObjects;
using Premedical.Services.Interfaces;

namespace Premedical.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ModeratorOrAdmin)]
public class EntityController : ControllerBase
{
    private readonly IEntityService _entityService;

    public EntityController(IEntityService entityService)
    {
        _entityService = entityService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EntityDTO>> GetEntity(Guid id) =>
        Ok(await _entityService.GetByIdAsync(id));

    [HttpGet("{id:guid}/children")]
    public async Task<ActionResult<EntityWithTranslationDTO>> GetEntityWithChildren(Guid id, [FromQuery] int langId) =>
        Ok(await _entityService.GetWithChildrenAsync(id, langId));

    [HttpPost]
    public async Task<ActionResult<EntityDTO>> CreateEntity(EntityPostDTO dto)
    {
        var entity = await _entityService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetEntity), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EntityDTO>> UpdateEntity(Guid id, EntityUpdateDTO dto) =>
        Ok(await _entityService.UpdateAsync(id, dto));

    [HttpPut("{id:guid}/move")]
    public async Task<ActionResult<EntityDTO>> MoveEntity(Guid id, EntityMoveDTO dto) =>
        Ok(await _entityService.MoveAsync(id, dto));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEntity(Guid id)
    {
        await _entityService.DeleteAsync(id);
        return NoContent();
    }
}
