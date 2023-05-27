namespace MediCaseAPI.DataTransferObjects;

public class EntityTranslationFileDTO
{
    public Guid Id { get; set; }
    public Guid TranslationId { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public DateTime UploadedAt { get; set; }
}
