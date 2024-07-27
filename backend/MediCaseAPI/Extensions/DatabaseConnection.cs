using Npgsql;

namespace MediCaseAPI.Extensions;

public static class DatabaseConnection
{
    /// <summary>
    /// The password can be kept apart from the connection string, e.g. injected by ECS from Secrets Manager.
    /// </summary>
    public const string PasswordKey = "Database:Password";

    public static string Get(IConfiguration configuration, string name)
    {
        var builder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString(name)
            ?? throw new InvalidOperationException($"Connection string '{name}' is missing."));

        var password = configuration[PasswordKey];
        if (!string.IsNullOrEmpty(password))
            builder.Password = password;

        return builder.ConnectionString;
    }
}
