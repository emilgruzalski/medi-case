namespace Premedical.Models;

public partial class EntitiesGraphDatum
{
    public Guid Id { get; set; }

    public Guid? ParentId { get; set; }

    public Guid ChildId { get; set; }

    public virtual Entity Child { get; set; } = null!;

    public virtual Entity? Parent { get; set; }
}
