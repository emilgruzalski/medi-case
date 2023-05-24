using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.Services;

/// <summary>
/// Compares the moderator database with the published one, so the administrator can review changes before publishing.
/// </summary>
public static class PendingChangesCalculator
{
    public static PendingChangesDTO Calculate(ContentSnapshot moderator, ContentSnapshot published, int langId)
    {
        var publishedEntities = published.Entities.ToDictionary(e => e.Id);
        var moderatorIds = moderator.Entities.Select(e => e.Id).ToHashSet();
        var changes = new List<PendingEntityChangeDTO>();

        foreach (var entity in moderator.Entities)
        {
            if (!publishedEntities.TryGetValue(entity.Id, out var publishedEntity))
                changes.Add(ToChange(entity, moderator, langId, PendingChangeType.Added));
            else if (entity.ModifiedAt != publishedEntity.ModifiedAt)
                changes.Add(ToChange(entity, moderator, langId, PendingChangeType.Modified));
        }

        changes.AddRange(published.Entities
            .Where(e => !moderatorIds.Contains(e.Id))
            .Select(e => ToChange(e, published, langId, PendingChangeType.Removed)));

        return new PendingChangesDTO
        {
            LanguagesChanged = !SameLanguages(moderator.Languages, published.Languages),
            Entities = changes.OrderByDescending(c => c.ModifiedAt).ToList()
        };
    }

    private static PendingEntityChangeDTO ToChange(Entity entity, ContentSnapshot snapshot, int langId, PendingChangeType type)
    {
        var translations = snapshot.Translations.Where(t => t.EntityId == entity.Id).ToList();
        var title = translations.FirstOrDefault(t => t.LangId == langId)?.Title
            ?? translations.OrderBy(t => t.LangId).FirstOrDefault()?.Title
            ?? EntityTreeBuilder.MissingTitle;

        return new PendingEntityChangeDTO
        {
            EntityId = entity.Id,
            TypeId = entity.TypeId,
            Title = title,
            ChangeType = type,
            ModifiedAt = entity.ModifiedAt
        };
    }

    private static bool SameLanguages(IEnumerable<EntityLanguage> first, IEnumerable<EntityLanguage> second) =>
        first.Select(l => (l.Id, l.LangValue, l.Name)).OrderBy(l => l.Id)
            .SequenceEqual(second.Select(l => (l.Id, l.LangValue, l.Name)).OrderBy(l => l.Id));
}
