using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Premedical.Authentication;
using Premedical.DataTransferObjects;
using Premedical.Repositories.Interfaces;
using Premedical.Services;

namespace Premedical.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ModeratorOrAdmin)]
public class EntitiesGraphDataController : ControllerBase
{
    private readonly IEntityGraphDataRepository _graphDataRepository;
    private readonly IMapper _mapper;

    public EntitiesGraphDataController(IEntityGraphDataRepository graphDataRepository, IMapper mapper)
    {
        _graphDataRepository = graphDataRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntityGraphDataDTO>>> GetGraphData() =>
        Ok(_mapper.Map<IEnumerable<EntityGraphDataDTO>>(await _graphDataRepository.GetAllAsync()));

    [HttpGet("tree")]
    public async Task<ActionResult<IEnumerable<EntityGraphObjectDTO>>> GetTree([FromQuery] int langId)
    {
        var entities = await _graphDataRepository.GetEntitiesForTreeAsync();
        return Ok(EntityTreeBuilder.Build(entities, langId));
    }
}
