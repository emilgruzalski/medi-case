using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Exceptions;
using Premedical.Models;
using Premedical.Repositories.Interfaces;
using Premedical.Services.Interfaces;

namespace Premedical.Services;

public class EntityService : IEntityService
{
    private readonly IEntityRepository _entityRepository;
    private readonly IEntityTypeRepository _typeRepository;
    private readonly IEntityLanguageRepository _languageRepository;
    private readonly IEntityGraphDataRepository _graphDataRepository;
    private readonly IMapper _mapper;

    public EntityService(
        IEntityRepository entityRepository,
        IEntityTypeRepository typeRepository,
        IEntityLanguageRepository languageRepository,
        IEntityGraphDataRepository graphDataRepository,
        IMapper mapper)
    {
        _entityRepository = entityRepository;
        _typeRepository = typeRepository;
        _languageRepository = languageRepository;
        _graphDataRepository = graphDataRepository;
        _mapper = mapper;
    }

    public async Task<EntityDTO> GetByIdAsync(Guid id)
    {
        var entity = await _entityRepository.GetWithDetailsAsync(id)
            ?? throw new NotFoundException($"Entity with id {id} was not found.");
        return _mapper.Map<EntityDTO>(entity);
    }

    public async Task<EntityDTO> CreateAsync(EntityPostDTO dto)
    {
        if (await _typeRepository.GetByIdAsync(dto.TypeId) is null)
            throw new BadRequestException($"Entity type with id {dto.TypeId} does not exist.");
        if (dto.ParentId is Guid parentId && !await _entityRepository.ExistsAsync(parentId))
            throw new BadRequestException($"Parent entity with id {parentId} does not exist.");

        await ValidateTranslationsAsync(dto.Translations);

        var now = DateTime.UtcNow;
        var entity = new Entity
        {
            Id = Guid.NewGuid(),
            TypeId = dto.TypeId,
            CreatedAt = now,
            ModifiedAt = now
        };
        entity.ParentLink = new EntitiesGraphDatum
        {
            Id = Guid.NewGuid(),
            ParentId = dto.ParentId,
            ChildId = entity.Id
        };

        foreach (var translationDto in dto.Translations)
        {
            var translation = _mapper.Map<EntityTranslation>(translationDto);
            translation.Id = Guid.NewGuid();
            translation.ModifiedAt = now;
            entity.EntityTranslations.Add(translation);
        }

        _entityRepository.Add(entity);
        await _entityRepository.SaveChangesAsync();

        return await GetByIdAsync(entity.Id);
    }

    public async Task<EntityWithTranslationDTO> GetWithChildrenAsync(Guid id, int langId)
    {
        var entities = await _graphDataRepository.GetEntitiesForTreeAsync();
        return EntityTreeBuilder.BuildSubtree(entities, id, langId)
            ?? throw new NotFoundException($"Entity with id {id} was not found.");
    }

    private async Task ValidateTranslationsAsync(IReadOnlyCollection<EntityTranslationContentDTO> translations)
    {
        if (translations.Select(t => t.LangId).Distinct().Count() != translations.Count)
            throw new BadRequestException("Each language can have only one translation.");

        foreach (var langId in translations.Select(t => t.LangId))
        {
            if (await _languageRepository.GetByIdAsync(langId) is null)
                throw new BadRequestException($"Language with id {langId} does not exist.");
        }
    }
}
