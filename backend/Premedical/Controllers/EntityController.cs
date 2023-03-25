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

    [HttpPost]
    public async Task<ActionResult<EntityDTO>> CreateEntity(EntityPostDTO dto)
    {
        var entity = await _entityService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetEntity), new { id = entity.Id }, entity);
    }
}
