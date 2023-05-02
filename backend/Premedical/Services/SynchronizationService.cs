using Microsoft.EntityFrameworkCore;
using Premedical.DataTransferObjects;
using Premedical.Models;
using Premedical.Services.Interfaces;

namespace Premedical.Services;

/// <summary>
/// Copies the content edited by moderators into the published database read by learners.
/// </summary>
public class SynchronizationService : ISynchronizationService
{
    private readonly MedicalContext _moderatorContext;
    private readonly IPublishedContextFactory _publishedContextFactory;
    private readonly IFileService _fileService;
    private readonly ILogger<SynchronizationService> _logger;

    public SynchronizationService(
        MedicalContext moderatorContext,
        IPublishedContextFactory publishedContextFactory,
        IFileService fileService,
        ILogger<SynchronizationService> logger)
    {
        _moderatorContext = moderatorContext;
        _publishedContextFactory = publishedContextFactory;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task<SynchronizationResultDTO> SynchronizeAsync()
    {
        var snapshot = await ContentSnapshot.LoadAsync(_moderatorContext);

        await using (var published = _publishedContextFactory.CreateDbContext())
        {
            // Learners never see a half-published state: either everything is replaced or nothing.
            await using var transaction = await published.Database.BeginTransactionAsync();
            await ClearContentAsync(published);
            snapshot.AddTo(published);
            await published.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        var result = snapshot.ToResult(DateTime.UtcNow);
        result.RemovedFiles = await RemoveUnusedFilesAsync(snapshot);

        _logger.LogInformation("Published {Entities} entities, removed {Files} unused files",
            result.Entities, result.RemovedFiles);
        return result;
    }

    private async Task<int> RemoveUnusedFilesAsync(ContentSnapshot snapshot)
    {
        // Files added by moderators after the snapshot was taken must stay on disk as well.
        var moderatorPaths = await _moderatorContext.EntityTranslationFiles.Select(f => f.FilePath).ToListAsync();
        var usedPaths = snapshot.Files.Select(f => f.FilePath).Concat(moderatorPaths).ToHashSet();
        return _fileService.DeleteUnusedFiles(usedPaths);
    }

    private static async Task ClearContentAsync(MedicalContext context)
    {
        context.EntityTranslationFiles.RemoveRange(await context.EntityTranslationFiles.ToListAsync());
        context.EntityDictionaries.RemoveRange(await context.EntityDictionaries.ToListAsync());
        context.EntityTranslations.RemoveRange(await context.EntityTranslations.ToListAsync());
        context.EntitiesGraphData.RemoveRange(await context.EntitiesGraphData.ToListAsync());
        context.Entities.RemoveRange(await context.Entities.ToListAsync());
        context.EntityLanguages.RemoveRange(await context.EntityLanguages.ToListAsync());
        context.EntityTypes.RemoveRange(await context.EntityTypes.ToListAsync());
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }
}
