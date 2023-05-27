namespace MediCaseAPI.Authentication;

public static class Roles
{
    public const string User = "User";
    public const string Moderator = "Moderator";
    public const string Admin = "Admin";

    public const string ModeratorOrAdmin = Moderator + "," + Admin;

    public static readonly IReadOnlyCollection<string> All = new[] { User, Moderator, Admin };
}
