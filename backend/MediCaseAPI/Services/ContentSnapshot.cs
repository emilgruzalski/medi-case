using Microsoft.EntityFrameworkCore;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Models;

namespace MediCaseAPI.Services;

/// <summary>
/// All content tables of one database, loaded without tracking so they can be added to another context.
/// </summary>
public class ContentSnapshot
{
    public List<EntityType> Types { get; private init; } = new();
    public List<EntityLanguage> Languages { get; private init; } = new();
    public List<Entity> Entities { get; private init; } = new();
    public List<EntitiesGraphDatum> GraphData { get; private init; } = new();
    public List<EntityTranslation> Translations { get; private init; } = new();
    public List<EntityTranslationFile> Files { get; private init; } = new();
    public List<EntityDictionary> Dictionary { get; private init; } = new();

    public static async Task<ContentSnapshot> LoadAsync(MedicalContext context) => new()
    {
        Types = await context.EntityTypes.AsNoTracking().ToListAsync(),
        Languages = await context.EntityLanguages.AsNoTracking().ToListAsync(),
        Entities = await context.Entities.AsNoTracking().ToListAsync(),
        GraphData = await context.EntitiesGraphData.AsNoTracking().ToListAsync(),
        Translations = await context.EntityTranslations.AsNoTracking().ToListAsync(),
        Files = await context.EntityTranslationFiles.AsNoTracking().ToListAsync(),
        Dictionary = await context.EntityDictionaries.AsNoTracking().ToListAsync()
    };

    public void AddTo(MedicalContext context)
    {
        context.EntityTypes.AddRange(Types);
        context.EntityLanguages.AddRange(Languages);
        context.Entities.AddRange(Entities);
        context.EntitiesGraphData.AddRange(GraphData);
        context.EntityTranslations.AddRange(Translations);
        context.EntityTranslationFiles.AddRange(Files);
        context.EntityDictionaries.AddRange(Dictionary);
    }

    public SynchronizationResultDTO ToResult(DateTime publishedAt) => new()
    {
        PublishedAt = publishedAt,
        Languages = Languages.Count,
        Entities = Entities.Count,
        Translations = Translations.Count,
        Files = Files.Count,
        DictionaryEntries = Dictionary.Count
    };
}
