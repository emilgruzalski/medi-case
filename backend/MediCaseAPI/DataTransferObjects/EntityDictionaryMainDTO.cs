namespace MediCaseAPI.DataTransferObjects;

public class EntityDictionaryMainDTO
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public int LangId { get; set; }
    public string Term { get; set; } = null!;
    public string Definition { get; set; } = null!;
}
