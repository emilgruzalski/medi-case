using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCaseAPI.Authentication;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Repositories.Interfaces;

namespace MediCaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ModeratorOrAdmin)]
public class EntityTypesController : ControllerBase
{
    private readonly IEntityTypeRepository _entityTypeRepository;
    private readonly IMapper _mapper;

    public EntityTypesController(IEntityTypeRepository entityTypeRepository, IMapper mapper)
    {
        _entityTypeRepository = entityTypeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntityTypeDTO>>> GetEntityTypes()
    {
        var types = await _entityTypeRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<EntityTypeDTO>>(types));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EntityTypeDTO>> GetEntityType(int id)
    {
        var type = await _entityTypeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Entity type with id {id} was not found.");
        return Ok(_mapper.Map<EntityTypeDTO>(type));
    }
}
