using System.Text;
using Amazon.Translate;
using Amazon.Translate.Model;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

public class AwsTextTranslator : ITextTranslator
{
    // Limit of a single TranslateText request.
    private const int MaxTextBytes = 10_000;

    private readonly IAmazonTranslate _translate;

    public AwsTextTranslator(IAmazonTranslate translate)
    {
        _translate = translate;
    }

    public async Task<string> TranslateTextAsync(string text, string sourceLanguage, string targetLanguage)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;
        if (Encoding.UTF8.GetByteCount(text) > MaxTextBytes)
            throw new BadRequestException("Text is too long to be translated in one request.");

        var response = await _translate.TranslateTextAsync(new TranslateTextRequest
        {
            Text = text,
            SourceLanguageCode = sourceLanguage,
            TargetLanguageCode = targetLanguage
        });

        return response.TranslatedText;
    }

    public async Task<string> TranslateHtmlAsync(string html, string sourceLanguage, string targetLanguage)
    {
        // TranslateText works on plain text, so only the text between tags is translated
        // and the markup produced by the editor stays untouched.
        var result = new StringBuilder();
        foreach (var segment in HtmlText.Split(html))
        {
            if (HtmlText.IsTag(segment) || string.IsNullOrWhiteSpace(segment))
            {
                result.Append(segment);
                continue;
            }

            var text = HtmlText.Decode(segment);
            var translated = await TranslateTextAsync(text.Trim(), sourceLanguage, targetLanguage);
            result.Append(LeadingWhitespace(text)).Append(HtmlText.Encode(translated)).Append(TrailingWhitespace(text));
        }

        return result.ToString();
    }

    private static string LeadingWhitespace(string text) => text[..(text.Length - text.TrimStart().Length)];

    private static string TrailingWhitespace(string text) => text[text.TrimEnd().Length..];
}
