using Premedical.Models;
using Premedical.Services;
using Premedical.Tests.Helpers;

namespace Premedical.Tests.Services;

public class EntityTreeBuilderTests
{
    [Fact]
    public void Build_SortsChildrenByOrder()
    {
        var root = EntityFactory.Create(EntityTypeIds.Category, null, "Root");
        var second = EntityFactory.Create(EntityTypeIds.Content, root.Id, "Second", order: 1);
        var first = EntityFactory.Create(EntityTypeIds.Content, root.Id, "First", order: 0);

        var tree = EntityTreeBuilder.Build(new[] { root, second, first }, langId: 1);

        Assert.Single(tree);
        Assert.Equal(new[] { "First", "Second" }, tree[0].Children.Select(c => c.Title));
    }

    [Fact]
    public void Build_UsesFallbackTitleWhenTranslationIsMissing()
    {
        var root = EntityFactory.Create(EntityTypeIds.Category, null, "Tylko polski");

        var node = EntityTreeBuilder.Build(new[] { root }, langId: 2).Single();

        Assert.Equal("Tylko polski", node.Title);
        Assert.False(node.HasTranslation);
    }

    [Fact]
    public void WithoutBlocked_RemovesBlockedEntitiesWithTheirSubtree()
    {
        var root = EntityFactory.Create(EntityTypeIds.Category, null, "Root");
        var blocked = EntityFactory.Create(EntityTypeIds.Category, root.Id, "Blocked", isBlocked: true);
        var hiddenChild = EntityFactory.Create(EntityTypeIds.Content, blocked.Id, "Hidden");
        var visible = EntityFactory.Create(EntityTypeIds.Content, root.Id, "Visible");

        var result = EntityTreeBuilder.WithoutBlocked(new[] { root, blocked, hiddenChild, visible });

        Assert.Equal(new[] { root.Id, visible.Id }, result.Select(e => e.Id));
    }
}
