namespace MediCaseAPI.Models;

public static class EntityTypeRules
{
    private static readonly int[] NoChildren = Array.Empty<int>();

    private static readonly IReadOnlyDictionary<int, int[]> AllowedChildren = new Dictionary<int, int[]>
    {
        [EntityTypeIds.Category] = new[]
        {
            EntityTypeIds.Category, EntityTypeIds.Content, EntityTypeIds.QuestionList, EntityTypeIds.Test
        },
        [EntityTypeIds.Content] = NoChildren,
        [EntityTypeIds.QuestionList] = new[] { EntityTypeIds.Question },
        [EntityTypeIds.Question] = NoChildren,
        [EntityTypeIds.Test] = new[] { EntityTypeIds.TestQuestion },
        [EntityTypeIds.TestQuestion] = new[] { EntityTypeIds.TestAnswer },
        [EntityTypeIds.TestAnswer] = NoChildren
    };

    public static readonly IReadOnlyCollection<int> RootTypes = new[] { EntityTypeIds.Category };

    public static IReadOnlyCollection<int> GetAllowedChildren(int typeId) =>
        AllowedChildren.TryGetValue(typeId, out var children) ? children : NoChildren;

    public static bool CanBeChildOf(int childTypeId, int? parentTypeId) =>
        parentTypeId is int parent
            ? GetAllowedChildren(parent).Contains(childTypeId)
            : RootTypes.Contains(childTypeId);
}
