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
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public EntityService(
        IEntityRepository entityRepository,
        IEntityTypeRepository typeRepository,
        IEntityLanguageRepository languageRepository,
        IEntityGraphDataRepository graphDataRepository,
        IFileService fileService,
        IMapper mapper)
    {
        _entityRepository = entityRepository;
        _typeRepository = typeRepository;
        _languageRepository = languageRepository;
        _graphDataRepository = graphDataRepository;
        _fileService = fileService;
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
        await ValidateParentAsync(dto.TypeId, dto.ParentId);
        ValidateCorrectFlag(dto.TypeId, dto.IsCorrect);
        await ValidateTranslationsAsync(dto.Translations);

        var now = DateTime.UtcNow;
        var entity = new Entity
        {
            Id = Guid.NewGuid(),
            TypeId = dto.TypeId,
            Order = dto.Order ?? await _entityRepository.GetMaxChildOrderAsync(dto.ParentId) + 1,
            IsCorrect = dto.IsCorrect,
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

    public async Task<EntityDTO> UpdateAsync(Guid id, EntityUpdateDTO dto)
    {
        var entity = await _entityRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Entity with id {id} was not found.");
        ValidateCorrectFlag(entity.TypeId, dto.IsCorrect);

        entity.Order = dto.Order;
        entity.IsCorrect = dto.IsCorrect;
        entity.ModifiedAt = DateTime.UtcNow;
        await _entityRepository.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<EntityDTO> MoveAsync(Guid id, EntityMoveDTO dto)
    {
        var entity = await _entityRepository.GetWithParentLinkAsync(id)
            ?? throw new NotFoundException($"Entity with id {id} was not found.");

        if (dto.ParentId is Guid parentId)
        {
            var subtreeIds = EntityTreeBuilder.CollectSubtreeIds(await _graphDataRepository.GetAllAsync(), id);
            if (subtreeIds.Contains(parentId))
                throw new BadRequestException("Entity cannot be moved into its own subtree.");
        }
        await ValidateParentAsync(entity.TypeId, dto.ParentId);

        entity.Order = dto.Order ?? await _entityRepository.GetMaxChildOrderAsync(dto.ParentId) + 1;
        entity.ParentLink!.ParentId = dto.ParentId;
        entity.ModifiedAt = DateTime.UtcNow;
        await _entityRepository.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (!await _entityRepository.ExistsAsync(id))
            throw new NotFoundException($"Entity with id {id} was not found.");

        var subtreeIds = EntityTreeBuilder.CollectSubtreeIds(await _graphDataRepository.GetAllAsync(), id);
        var entities = await _entityRepository.GetByIdsAsync(subtreeIds);
        var filePaths = entities
            .SelectMany(e => e.EntityTranslations)
            .SelectMany(t => t.EntityTranslationFiles)
            .Select(f => f.FilePath)
            .ToList();

        _entityRepository.RemoveRange(entities);
        await _entityRepository.SaveChangesAsync();

        foreach (var path in filePaths)
            _fileService.Delete(path);
    }

    private async Task ValidateParentAsync(int typeId, Guid? parentId)
    {
        int? parentTypeId = null;
        if (parentId is Guid id)
        {
            var parent = await _entityRepository.GetByIdAsync(id)
                ?? throw new BadRequestException($"Parent entity with id {id} does not exist.");
            parentTypeId = parent.TypeId;
        }

        if (!EntityTypeRules.CanBeChildOf(typeId, parentTypeId))
            throw new BadRequestException("This entity type is not allowed in the selected place of the tree.");
    }

    private static void ValidateCorrectFlag(int typeId, bool isCorrect)
    {
        if (isCorrect && typeId != EntityTypeIds.TestAnswer)
            throw new BadRequestException("Only test answers can be marked as correct.");
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
