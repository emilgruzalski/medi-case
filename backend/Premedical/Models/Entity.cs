namespace Premedical.Models;

public partial class Entity
{
    public Guid Id { get; set; }

    public int TypeId { get; set; }

    public int Order { get; set; }

    public bool IsCorrect { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual EntityType Type { get; set; } = null!;

    public virtual EntitiesGraphDatum? ParentLink { get; set; }

    public virtual ICollection<EntitiesGraphDatum> ChildLinks { get; set; } = new List<EntitiesGraphDatum>();

    public virtual ICollection<EntityDictionary> EntityDictionaries { get; set; } = new List<EntityDictionary>();

    public virtual ICollection<EntityTranslation> EntityTranslations { get; set; } = new List<EntityTranslation>();
}
