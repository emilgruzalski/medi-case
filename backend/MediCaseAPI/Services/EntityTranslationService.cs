using AutoMapper;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories.Interfaces;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

public class EntityTranslationService : IEntityTranslationService
{
    private readonly IEntityTranslationsRepository _translationsRepository;
    private readonly IEntityRepository _entityRepository;
    private readonly IEntityLanguageRepository _languageRepository;
    private readonly IMapper _mapper;

    public EntityTranslationService(
        IEntityTranslationsRepository translationsRepository,
        IEntityRepository entityRepository,
        IEntityLanguageRepository languageRepository,
        IMapper mapper)
    {
        _translationsRepository = translationsRepository;
        _entityRepository = entityRepository;
        _languageRepository = languageRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EntityTranslationDTO>> GetByEntityAsync(Guid entityId) =>
        _mapper.Map<IEnumerable<EntityTranslationDTO>>(await _translationsRepository.GetByEntityAsync(entityId));

    public async Task<EntityTranslationDTO> GetByIdAsync(Guid id) =>
        _mapper.Map<EntityTranslationDTO>(await GetTranslationOrThrowAsync(id));

    public async Task<EntityTranslationDTO> CreateAsync(EntityTranslationPostDTO dto)
    {
        var entity = await _entityRepository.GetByIdAsync(dto.EntityId)
            ?? throw new NotFoundException($"Entity with id {dto.EntityId} was not found.");
        if (await _languageRepository.GetByIdAsync(dto.LangId) is null)
            throw new BadRequestException($"Language with id {dto.LangId} does not exist.");
        if (await _translationsRepository.ExistsAsync(dto.EntityId, dto.LangId))
            throw new ConflictException("Translation for this language already exists.");

        var translation = _mapper.Map<EntityTranslation>(dto);
        translation.ModifiedAt = DateTime.UtcNow;
        entity.ModifiedAt = translation.ModifiedAt;

        _translationsRepository.Add(translation);
        await _translationsRepository.SaveChangesAsync();

        return await GetByIdAsync(translation.Id);
    }

    public async Task<EntityTranslationDTO> UpdateAsync(Guid id, EntityTranslationUpdateDTO dto)
    {
        var translation = await GetTranslationOrThrowAsync(id);
        _mapper.Map(dto, translation);
        translation.ModifiedAt = DateTime.UtcNow;
        translation.Entity.ModifiedAt = translation.ModifiedAt;

        await _translationsRepository.SaveChangesAsync();
        return _mapper.Map<EntityTranslationDTO>(translation);
    }

    public async Task DeleteAsync(Guid id)
    {
        var translation = await GetTranslationOrThrowAsync(id);
        translation.Entity.ModifiedAt = DateTime.UtcNow;

        _translationsRepository.Remove(translation);
        await _translationsRepository.SaveChangesAsync();
    }

    private async Task<EntityTranslation> GetTranslationOrThrowAsync(Guid id) =>
        await _translationsRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"Translation with id {id} was not found.");
}
