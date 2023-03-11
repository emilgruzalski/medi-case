using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Premedical.Authentication;
using Premedical.DataTransferObjects;
using Premedical.Exceptions;
using Premedical.Repositories.Interfaces;

namespace Premedical.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ModeratorOrAdmin)]
public class EntityController : ControllerBase
{
    private readonly IEntityRepository _entityRepository;
    private readonly IMapper _mapper;

    public EntityController(IEntityRepository entityRepository, IMapper mapper)
    {
        _entityRepository = entityRepository;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EntityDTO>> GetEntity(Guid id)
    {
        var entity = await _entityRepository.GetWithDetailsAsync(id)
            ?? throw new NotFoundException($"Entity with id {id} was not found.");
        return Ok(_mapper.Map<EntityDTO>(entity));
    }
}
