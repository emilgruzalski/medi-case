using MediCaseAPI.Authentication;

namespace MediCaseAPI.Tests.Authentication;

public class RoleResolverTests
{
    [Theory]
    [InlineData(new string[0], Roles.User)]
    [InlineData(new[] { "Moderator" }, Roles.Moderator)]
    [InlineData(new[] { "Moderator", "Admin" }, Roles.Admin)]
    [InlineData(new[] { "Students" }, Roles.User)]
    public void FromGroups_ReturnsHighestRole(string[] groups, string expected) =>
        Assert.Equal(expected, RoleResolver.FromGroups(groups));
}
