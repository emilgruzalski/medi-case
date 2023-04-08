namespace Premedical.Models;

public partial class EntityTranslationFile
{
    public Guid Id { get; set; }

    public Guid TranslationId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long Size { get; set; }

    public DateTime UploadedAt { get; set; }

    public virtual EntityTranslation Translation { get; set; } = null!;
}
