using Premedical.Models;

namespace Premedical.Tests.Models;

public class EntityTypeRulesTests
{
    [Theory]
    [InlineData(EntityTypeIds.Category, true)]
    [InlineData(EntityTypeIds.Content, false)]
    [InlineData(EntityTypeIds.Question, false)]
    public void CanBeChildOf_RootLevel_AllowsOnlyCategories(int typeId, bool expected) =>
        Assert.Equal(expected, EntityTypeRules.CanBeChildOf(typeId, null));

    [Theory]
    [InlineData(EntityTypeIds.Content, EntityTypeIds.Category, true)]
    [InlineData(EntityTypeIds.Question, EntityTypeIds.QuestionList, true)]
    [InlineData(EntityTypeIds.TestAnswer, EntityTypeIds.TestQuestion, true)]
    [InlineData(EntityTypeIds.Question, EntityTypeIds.Category, false)]
    [InlineData(EntityTypeIds.Content, EntityTypeIds.Content, false)]
    public void CanBeChildOf_ChecksAllowedChildren(int childType, int parentType, bool expected) =>
        Assert.Equal(expected, EntityTypeRules.CanBeChildOf(childType, parentType));
}
