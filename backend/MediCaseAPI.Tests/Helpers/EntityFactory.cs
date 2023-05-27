using MediCaseAPI.Models;

namespace MediCaseAPI.Tests.Helpers;

public static class EntityFactory
{
    public static Entity Create(int typeId, Guid? parentId, string titlePl, int order = 0, bool isBlocked = false)
    {
        var id = Guid.NewGuid();
        var now = DateTime.UtcNow;
        return new Entity
        {
            Id = id,
            TypeId = typeId,
            Order = order,
            IsBlocked = isBlocked,
            CreatedAt = now,
            ModifiedAt = now,
            ParentLink = new EntitiesGraphDatum { Id = Guid.NewGuid(), ParentId = parentId, ChildId = id },
            EntityTranslations =
            {
                new EntityTranslation { Id = Guid.NewGuid(), EntityId = id, LangId = 1, Title = titlePl, ModifiedAt = now }
            }
        };
    }
}
