using MediCaseAPI.Services;

namespace MediCaseAPI.Tests.Services;

public class HtmlTextTests
{
    [Fact]
    public void ToPlainText_RemovesTagsAndDecodesEntities() =>
        Assert.Equal("Schemat ABCDE: A & B", HtmlText.ToPlainText("<p>Schemat <strong>ABCDE</strong>:</p><p>A &amp; B</p>"));

    [Fact]
    public void Split_KeepsTagsAsSeparateSegments() =>
        Assert.Equal(new[] { "", "<p>", "Tekst", "</p>", "" }, HtmlText.Split("<p>Tekst</p>"));
}
