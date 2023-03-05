using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Exceptions;
using Premedical.Models;
using Premedical.Repositories.Interfaces;
using Premedical.Services.Interfaces;

namespace Premedical.Services;

public class EntityLanguageService : IEntityLanguageService
{
    private readonly IEntityLanguageRepository _languageRepository;
    private readonly IMapper _mapper;

    public EntityLanguageService(IEntityLanguageRepository languageRepository, IMapper mapper)
    {
        _languageRepository = languageRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EntityLanguageGetDTO>> GetAllAsync() =>
        _mapper.Map<IEnumerable<EntityLanguageGetDTO>>(await _languageRepository.GetAllAsync());

    public async Task<EntityLanguageGetDTO> GetByIdAsync(int id) =>
        _mapper.Map<EntityLanguageGetDTO>(await GetLanguageOrThrowAsync(id));

    public async Task<EntityLanguageGetDTO> CreateAsync(EntityLanguagePostDTO dto)
    {
        var language = _mapper.Map<EntityLanguage>(dto);
        if (await _languageRepository.LangValueExistsAsync(language.LangValue))
            throw new ConflictException($"Language '{language.LangValue}' already exists.");

        // Ids are assigned here so that both databases keep the same language ids.
        language.Id = await _languageRepository.GetMaxIdAsync() + 1;

        _languageRepository.Add(language);
        await _languageRepository.SaveChangesAsync();

        return _mapper.Map<EntityLanguageGetDTO>(language);
    }

    public async Task<EntityLanguageGetDTO> UpdateAsync(int id, EntityLanguageUpdateDTO dto)
    {
        var language = await GetLanguageOrThrowAsync(id);
        _mapper.Map(dto, language);

        if (await _languageRepository.LangValueExistsAsync(language.LangValue, id))
            throw new ConflictException($"Language '{language.LangValue}' already exists.");

        await _languageRepository.SaveChangesAsync();
        return _mapper.Map<EntityLanguageGetDTO>(language);
    }

    public async Task DeleteAsync(int id)
    {
        var language = await GetLanguageOrThrowAsync(id);
        if (await _languageRepository.IsUsedAsync(id))
            throw new ConflictException("Language is used by translations or dictionary entries.");

        _languageRepository.Remove(language);
        await _languageRepository.SaveChangesAsync();
    }

    private async Task<EntityLanguage> GetLanguageOrThrowAsync(int id) =>
        await _languageRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"Language with id {id} was not found.");
}
