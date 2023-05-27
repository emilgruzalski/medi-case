namespace MediCaseAPI.Options;

public class SpeechOptions
{
    public const string SectionName = "Speech";

    public string Engine { get; set; } = null!;
    public int MaxCharacters { get; set; }
    public Dictionary<string, string> Voices { get; set; } = new();
}
