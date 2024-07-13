namespace MediCaseAPI.Authentication;

/// <summary>Maps Cognito groups to the application role. Users without a group are learners.</summary>
public static class RoleResolver
{
    public static string FromGroups(IEnumerable<string> groups)
    {
        var set = groups.ToHashSet(StringComparer.Ordinal);
        if (set.Contains(Roles.Admin))
            return Roles.Admin;
        return set.Contains(Roles.Moderator) ? Roles.Moderator : Roles.User;
    }
}
