namespace MediCaseAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? CognitoUsername { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
