namespace Premedical.DataTransferObjects;

public class EntityWithTranslationDTO
{
    public Guid Id { get; set; }
    public int TypeId { get; set; }
    public int LangId { get; set; }
    public Guid? TranslationId { get; set; }
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public bool HasTranslation { get; set; }
    public List<EntityWithTranslationDTO> Children { get; set; } = new();
}
