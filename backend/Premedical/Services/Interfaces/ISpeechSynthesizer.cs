namespace Premedical.Services.Interfaces;

public interface ISpeechSynthesizer
{
    Task<Stream> SynthesizeAsync(string text, string languageCode);
}
