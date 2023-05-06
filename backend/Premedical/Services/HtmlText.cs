using System.Net;
using System.Text.RegularExpressions;

namespace Premedical.Services;

public static class HtmlText
{
    private static readonly Regex TagPattern = new("(<[^>]+>)", RegexOptions.Compiled);
    private static readonly Regex WhitespacePattern = new(@"\s+", RegexOptions.Compiled);

    /// <summary>Splits HTML into tags and text segments, keeping both.</summary>
    public static string[] Split(string html) => TagPattern.Split(html);

    public static bool IsTag(string segment) => segment.StartsWith('<') && segment.EndsWith('>');

    public static string Decode(string text) => WebUtility.HtmlDecode(text);

    public static string Encode(string text) =>
        text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");

    public static string ToPlainText(string? html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        var withoutTags = TagPattern.Replace(html, " ");
        return WhitespacePattern.Replace(Decode(withoutTags), " ").Trim();
    }
}
