namespace MediCaseAPI.DataTransferObjects;

public class EntityLanguageGetDTO
{
    public int Id { get; set; }
    public string LangValue { get; set; } = null!;
    public string Name { get; set; } = null!;
}
