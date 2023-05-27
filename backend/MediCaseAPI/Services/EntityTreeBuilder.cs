using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Models;

namespace MediCaseAPI.Services;

public static class EntityTreeBuilder
{
    public const string MissingTitle = "(no title)";

    public static List<EntityGraphObjectDTO> Build(IReadOnlyCollection<Entity> entities, int langId, bool includeBlocked = true)
    {
        var children = ToChildrenLookup(includeBlocked ? entities : WithoutBlocked(entities));
        return BuildGraphNodes(children, null, langId);
    }

    public static EntityWithTranslationDTO? BuildSubtree(
        IReadOnlyCollection<Entity> entities, Guid rootId, int langId, bool includeBlocked = true)
    {
        var visible = includeBlocked ? entities : WithoutBlocked(entities);
        var root = visible.FirstOrDefault(e => e.Id == rootId);
        return root is null ? null : BuildContentNode(root, ToChildrenLookup(visible), langId);
    }

    /// <summary>Removes blocked entities together with everything below them.</summary>
    public static IReadOnlyCollection<Entity> WithoutBlocked(IReadOnlyCollection<Entity> entities)
    {
        var edges = entities.Where(e => e.ParentLink != null).Select(e => e.ParentLink!).ToList();
        var hidden = entities
            .Where(e => e.IsBlocked)
            .SelectMany(e => CollectSubtreeIds(edges, e.Id))
            .ToHashSet();
        return entities.Where(e => !hidden.Contains(e.Id)).ToList();
    }

    public static List<Guid> CollectSubtreeIds(IEnumerable<EntitiesGraphDatum> edges, Guid rootId)
    {
        var children = edges.ToLookup(e => e.ParentId, e => e.ChildId);
        var result = new List<Guid>();
        var pending = new Stack<Guid>();
        pending.Push(rootId);

        while (pending.Count > 0)
        {
            var id = pending.Pop();
            result.Add(id);
            foreach (var childId in children[id])
                pending.Push(childId);
        }

        return result;
    }

    private static ILookup<Guid?, Entity> ToChildrenLookup(IEnumerable<Entity> entities) =>
        entities.ToLookup(e => e.ParentLink?.ParentId);

    private static IEnumerable<Entity> Sorted(IEnumerable<Entity> entities) =>
        entities.OrderBy(e => e.Order).ThenBy(e => e.CreatedAt);

    private static List<EntityGraphObjectDTO> BuildGraphNodes(ILookup<Guid?, Entity> children, Guid? parentId, int langId) =>
        Sorted(children[parentId])
            .Select(e =>
            {
                var (translation, title) = SelectTranslation(e, langId);
                return new EntityGraphObjectDTO
                {
                    Id = e.Id,
                    TypeId = e.TypeId,
                    Order = e.Order,
                    IsBlocked = e.IsBlocked,
                    Title = title,
                    HasTranslation = translation is not null,
                    Children = BuildGraphNodes(children, e.Id, langId)
                };
            })
            .ToList();

    private static EntityWithTranslationDTO BuildContentNode(Entity entity, ILookup<Guid?, Entity> children, int langId)
    {
        var (translation, title) = SelectTranslation(entity, langId);
        return new EntityWithTranslationDTO
        {
            Id = entity.Id,
            TypeId = entity.TypeId,
            Order = entity.Order,
            IsCorrect = entity.IsCorrect,
            IsBlocked = entity.IsBlocked,
            LangId = langId,
            TranslationId = translation?.Id,
            Title = title,
            Content = translation?.Content,
            HasTranslation = translation is not null,
            Files = translation?.EntityTranslationFiles.OrderBy(f => f.UploadedAt).Select(ToFileDto).ToList() ?? new(),
            Children = Sorted(children[entity.Id]).Select(c => BuildContentNode(c, children, langId)).ToList()
        };
    }

    private static EntityTranslationFileDTO ToFileDto(EntityTranslationFile file) => new()
    {
        Id = file.Id,
        TranslationId = file.TranslationId,
        FileName = file.FileName,
        ContentType = file.ContentType,
        Size = file.Size,
        UploadedAt = file.UploadedAt
    };

    private static (EntityTranslation? Translation, string Title) SelectTranslation(Entity entity, int langId)
    {
        var translation = entity.EntityTranslations.FirstOrDefault(t => t.LangId == langId);
        var fallback = entity.EntityTranslations.OrderBy(t => t.LangId).FirstOrDefault();
        return (translation, translation?.Title ?? fallback?.Title ?? MissingTitle);
    }
}
