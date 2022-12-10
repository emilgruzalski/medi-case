namespace Premedical.Models;

public partial class EntityTranslation
{
    public Guid Id { get; set; }

    public Guid EntityId { get; set; }

    public int LangId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    public virtual EntityLanguage Lang { get; set; } = null!;
}
