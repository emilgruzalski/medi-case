namespace Premedical.DataTransferObjects;

public class EntityGraphObjectDTO
{
    public Guid Id { get; set; }
    public int TypeId { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = null!;
    public bool HasTranslation { get; set; }
    public List<EntityGraphObjectDTO> Children { get; set; } = new();
}
