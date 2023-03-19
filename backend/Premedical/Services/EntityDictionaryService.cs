using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Exceptions;
using Premedical.Models;
using Premedical.Repositories.Interfaces;
using Premedical.Services.Interfaces;

namespace Premedical.Services;

public class EntityDictionaryService : IEntityDictionaryService
{
    private readonly IEntityDictionaryRepository _dictionaryRepository;
    private readonly IEntityRepository _entityRepository;
    private readonly IEntityLanguageRepository _languageRepository;
    private readonly IMapper _mapper;

    public EntityDictionaryService(
        IEntityDictionaryRepository dictionaryRepository,
        IEntityRepository entityRepository,
        IEntityLanguageRepository languageRepository,
        IMapper mapper)
    {
        _dictionaryRepository = dictionaryRepository;
        _entityRepository = entityRepository;
        _languageRepository = languageRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EntityDictionaryMainDTO>> GetByEntityAsync(Guid entityId, int? langId) =>
        _mapper.Map<IEnumerable<EntityDictionaryMainDTO>>(await _dictionaryRepository.GetByEntityAsync(entityId, langId));

    public async Task<EntityDictionaryMainDTO> GetByIdAsync(Guid id) =>
        _mapper.Map<EntityDictionaryMainDTO>(await GetEntryOrThrowAsync(id));

    public async Task<EntityDictionaryMainDTO> CreateAsync(EntityDictionaryPostDTO dto)
    {
        var entity = await _entityRepository.GetByIdAsync(dto.EntityId)
            ?? throw new NotFoundException($"Entity with id {dto.EntityId} was not found.");
        if (await _languageRepository.GetByIdAsync(dto.LangId) is null)
            throw new BadRequestException($"Language with id {dto.LangId} does not exist.");

        var entry = _mapper.Map<EntityDictionary>(dto);
        entity.ModifiedAt = DateTime.UtcNow;

        _dictionaryRepository.Add(entry);
        await _dictionaryRepository.SaveChangesAsync();

        return _mapper.Map<EntityDictionaryMainDTO>(entry);
    }

    public async Task<EntityDictionaryMainDTO> UpdateAsync(Guid id, EntityDictionaryUpdateDTO dto)
    {
        var entry = await GetEntryOrThrowAsync(id);
        _mapper.Map(dto, entry);
        entry.Entity.ModifiedAt = DateTime.UtcNow;

        await _dictionaryRepository.SaveChangesAsync();
        return _mapper.Map<EntityDictionaryMainDTO>(entry);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entry = await GetEntryOrThrowAsync(id);
        entry.Entity.ModifiedAt = DateTime.UtcNow;

        _dictionaryRepository.Remove(entry);
        await _dictionaryRepository.SaveChangesAsync();
    }

    private async Task<EntityDictionary> GetEntryOrThrowAsync(Guid id) =>
        await _dictionaryRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"Dictionary entry with id {id} was not found.");
}
