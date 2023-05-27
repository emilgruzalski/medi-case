namespace MediCaseAPI.DataTransferObjects;

public class EntityGraphDataDTO
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public Guid ChildId { get; set; }
}
