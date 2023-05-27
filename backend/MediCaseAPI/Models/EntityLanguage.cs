namespace MediCaseAPI.Models;

public partial class EntityLanguage
{
    public int Id { get; set; }

    public string LangValue { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<EntityDictionary> EntityDictionaries { get; set; } = new List<EntityDictionary>();

    public virtual ICollection<EntityTranslation> EntityTranslations { get; set; } = new List<EntityTranslation>();
}
