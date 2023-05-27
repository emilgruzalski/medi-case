namespace MediCaseAPI.Options;

public class SessionTokenOptions
{
    public const string SectionName = "Sessions";

    public int LifetimeHours { get; set; }
    public int TokenBytes { get; set; }
}
