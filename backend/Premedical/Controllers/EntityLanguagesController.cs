using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Premedical.Authentication;
using Premedical.DataTransferObjects;
using Premedical.Services.Interfaces;

namespace Premedical.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ModeratorOrAdmin)]
public class EntityLanguagesController : ControllerBase
{
    private readonly IEntityLanguageService _languageService;

    public EntityLanguagesController(IEntityLanguageService languageService)
    {
        _languageService = languageService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntityLanguageGetDTO>>> GetLanguages() =>
        Ok(await _languageService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EntityLanguageGetDTO>> GetLanguage(int id) =>
        Ok(await _languageService.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<EntityLanguageGetDTO>> CreateLanguage(EntityLanguagePostDTO dto)
    {
        var language = await _languageService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetLanguage), new { id = language.Id }, language);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EntityLanguageGetDTO>> UpdateLanguage(int id, EntityLanguageUpdateDTO dto) =>
        Ok(await _languageService.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLanguage(int id)
    {
        await _languageService.DeleteAsync(id);
        return NoContent();
    }
}
