namespace Premedical.DataTransferObjects;

public class EntityTypeDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool CanBeRoot { get; set; }
    public List<int> AllowedChildTypeIds { get; set; } = new();
}
