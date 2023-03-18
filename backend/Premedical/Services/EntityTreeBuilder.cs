using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.Services;

public static class EntityTreeBuilder
{
    public const string MissingTitle = "(no title)";

    public static List<EntityGraphObjectDTO> Build(IReadOnlyCollection<Entity> entities, int langId)
    {
        var nodes = entities.ToDictionary(e => e.Id, e => CreateNode(e, langId));
        var roots = new List<EntityGraphObjectDTO>();

        foreach (var entity in entities.OrderBy(e => e.CreatedAt))
        {
            var node = nodes[entity.Id];
            var parentId = entity.ParentLink?.ParentId;

            if (parentId is Guid id && nodes.TryGetValue(id, out var parent))
                parent.Children.Add(node);
            else
                roots.Add(node);
        }

        return roots;
    }

    private static EntityGraphObjectDTO CreateNode(Entity entity, int langId)
    {
        var translation = entity.EntityTranslations.FirstOrDefault(t => t.LangId == langId);
        var fallback = entity.EntityTranslations.OrderBy(t => t.LangId).FirstOrDefault();

        return new EntityGraphObjectDTO
        {
            Id = entity.Id,
            TypeId = entity.TypeId,
            Title = translation?.Title ?? fallback?.Title ?? MissingTitle,
            HasTranslation = translation is not null
        };
    }
}
