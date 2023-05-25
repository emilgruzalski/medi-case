namespace Premedical.DataTransferObjects;

public class EntitySearchResultDTO
{
    public Guid EntityId { get; set; }
    public int TypeId { get; set; }
    public string Title { get; set; } = null!;
    public List<Guid> PathIds { get; set; } = new();
    public List<string> Path { get; set; } = new();
}
