namespace MediCaseAPI.Services.Interfaces;

public interface ITextTranslator
{
    Task<string> TranslateTextAsync(string text, string sourceLanguage, string targetLanguage);
    Task<string> TranslateHtmlAsync(string html, string sourceLanguage, string targetLanguage);
}
