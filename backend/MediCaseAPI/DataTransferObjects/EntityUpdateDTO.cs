namespace MediCaseAPI.DataTransferObjects;

public class EntityUpdateDTO
{
    public int Order { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsBlocked { get; set; }
}
