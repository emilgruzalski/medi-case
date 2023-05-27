namespace MediCaseAPI.DataTransferObjects;

public class SessionDTO
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public UserDTO User { get; set; } = null!;
}
