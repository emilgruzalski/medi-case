namespace Premedical.DataTransferObjects;

public class EntityDTO
{
    public Guid Id { get; set; }
    public int TypeId { get; set; }
    public int Order { get; set; }
    public bool IsCorrect { get; set; }
    public string TypeName { get; set; } = null!;
    public Guid? ParentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public List<EntityTranslationDTO> Translations { get; set; } = new();
}
