using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Models;

namespace MediCaseAPI.Services;

public static class EntitySearch
{
    public const int MinQueryLength = 2;
    public const int MaxResults = 20;

    public static List<EntitySearchResultDTO> Search(IReadOnlyCollection<Entity> entities, string? query, int langId)
    {
        var text = query?.Trim() ?? string.Empty;
        if (text.Length < MinQueryLength)
            throw new BadRequestException($"Search query must have at least {MinQueryLength} characters.");

        var byId = entities.ToDictionary(e => e.Id);
        return entities
            .Select(e => (Entity: e, Translation: e.EntityTranslations.FirstOrDefault(t => t.LangId == langId)))
            .Where(x => x.Translation is not null &&
                        x.Translation.Title.Contains(text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(x => x.Translation!.Title)
            .Take(MaxResults)
            .Select(x => ToResult(x.Entity, x.Translation!, byId, langId))
            .ToList();
    }

    private static EntitySearchResultDTO ToResult(
        Entity entity, EntityTranslation translation, IReadOnlyDictionary<Guid, Entity> byId, int langId)
    {
        var ancestors = new List<Entity>();
        var parentId = entity.ParentLink?.ParentId;
        while (parentId is Guid id && byId.TryGetValue(id, out var parent))
        {
            ancestors.Insert(0, parent);
            parentId = parent.ParentLink?.ParentId;
        }

        return new EntitySearchResultDTO
        {
            EntityId = entity.Id,
            TypeId = entity.TypeId,
            Title = translation.Title,
            PathIds = ancestors.Select(a => a.Id).ToList(),
            Path = ancestors
                .Select(a => a.EntityTranslations.FirstOrDefault(t => t.LangId == langId)?.Title ?? EntityTreeBuilder.MissingTitle)
                .ToList()
        };
    }
}
