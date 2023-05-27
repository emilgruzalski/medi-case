namespace MediCaseAPI.Models;

public partial class EntityDictionary
{
    public Guid Id { get; set; }

    public Guid EntityId { get; set; }

    public int LangId { get; set; }

    public string Term { get; set; } = null!;

    public string Definition { get; set; } = null!;

    public virtual Entity Entity { get; set; } = null!;

    public virtual EntityLanguage Lang { get; set; } = null!;
}
