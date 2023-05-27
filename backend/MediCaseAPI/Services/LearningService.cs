using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

/// <summary>
/// Read-only access for learners. Everything comes from the published database.
/// </summary>
public class LearningService : ILearningService
{
    private readonly IPublishedContextFactory _publishedContextFactory;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public LearningService(IPublishedContextFactory publishedContextFactory, IFileService fileService, IMapper mapper)
    {
        _publishedContextFactory = publishedContextFactory;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EntityLanguageGetDTO>> GetLanguagesAsync()
    {
        await using var context = _publishedContextFactory.CreateDbContext();
        var languages = await new EntityLanguageRepository(context).GetAllAsync();
        return _mapper.Map<IEnumerable<EntityLanguageGetDTO>>(languages);
    }

    public async Task<IEnumerable<EntityGraphObjectDTO>> GetTreeAsync(int langId)
    {
        await using var context = _publishedContextFactory.CreateDbContext();
        await EnsureLanguageExistsAsync(context, langId);

        var entities = await new EntityGraphDataRepository(context).GetEntitiesForTreeAsync();
        return EntityTreeBuilder.Build(entities, langId, includeBlocked: false);
    }

    public async Task<EntityWithTranslationDTO> GetEntityAsync(Guid id, int langId)
    {
        await using var context = _publishedContextFactory.CreateDbContext();
        await EnsureLanguageExistsAsync(context, langId);

        var entities = await new EntityGraphDataRepository(context).GetEntitiesForTreeAsync();
        return EntityTreeBuilder.BuildSubtree(entities, id, langId, includeBlocked: false)
            ?? throw new NotFoundException($"Entity with id {id} was not found.");
    }

    public async Task<IEnumerable<EntityDictionaryMainDTO>> GetDictionaryAsync(Guid entityId, int langId)
    {
        await using var context = _publishedContextFactory.CreateDbContext();
        var entries = await new EntityDictionaryRepository(context).GetByEntityAsync(entityId, langId);
        return _mapper.Map<IEnumerable<EntityDictionaryMainDTO>>(entries);
    }

    public async Task<FileDownload> DownloadFileAsync(Guid fileId)
    {
        await using var context = _publishedContextFactory.CreateDbContext();
        var file = await context.EntityTranslationFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == fileId)
            ?? throw new NotFoundException($"File with id {fileId} was not found.");

        return new FileDownload(_fileService.OpenRead(file.FilePath), file.ContentType, file.FileName);
    }

    public async Task<IEnumerable<EntitySearchResultDTO>> SearchAsync(string? query, int langId)
    {
        await using var context = _publishedContextFactory.CreateDbContext();
        var entities = await new EntityGraphDataRepository(context).GetEntitiesForTreeAsync();
        return EntitySearch.Search(EntityTreeBuilder.WithoutBlocked(entities), query, langId);
    }

    private static async Task EnsureLanguageExistsAsync(MedicalContext context, int langId)
    {
        if (!await context.EntityLanguages.AnyAsync(l => l.Id == langId))
            throw new BadRequestException($"Language with id {langId} does not exist.");
    }
}
