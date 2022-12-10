namespace Premedical.Models;

public partial class Session
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public virtual User User { get; set; } = null!;
}
