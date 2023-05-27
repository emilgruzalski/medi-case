namespace MediCaseAPI.DataTransferObjects;

public class EntityWithTranslationDTO
{
    public Guid Id { get; set; }
    public int TypeId { get; set; }
    public int Order { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsBlocked { get; set; }
    public int LangId { get; set; }
    public Guid? TranslationId { get; set; }
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public bool HasTranslation { get; set; }
    public List<EntityTranslationFileDTO> Files { get; set; } = new();
    public List<EntityWithTranslationDTO> Children { get; set; } = new();
}
