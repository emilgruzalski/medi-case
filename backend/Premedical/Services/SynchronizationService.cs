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
    private readonly ILogger<SynchronizationService> _logger;

    public SynchronizationService(
        MedicalContext moderatorContext,
        IPublishedContextFactory publishedContextFactory,
        ILogger<SynchronizationService> logger)
    {
        _moderatorContext = moderatorContext;
        _publishedContextFactory = publishedContextFactory;
        _logger = logger;
    }

    public async Task<SynchronizationResultDTO> SynchronizeAsync()
    {
        var snapshot = await ContentSnapshot.LoadAsync(_moderatorContext);

        await using var published = _publishedContextFactory.CreateDbContext();
        await ClearContentAsync(published);
        snapshot.AddTo(published);
        await published.SaveChangesAsync();

        _logger.LogInformation("Published {Count} entities", snapshot.Entities.Count);
        return snapshot.ToResult(DateTime.UtcNow);
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
