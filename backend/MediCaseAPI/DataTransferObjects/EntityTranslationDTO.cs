namespace MediCaseAPI.DataTransferObjects;

public class EntityTranslationDTO
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public int LangId { get; set; }
    public string LangValue { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public DateTime ModifiedAt { get; set; }
    public List<EntityTranslationFileDTO> Files { get; set; } = new();
}
