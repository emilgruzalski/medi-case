using System.ComponentModel.DataAnnotations;

namespace Premedical.DataTransferObjects;

public class EntityPostDTO
{
    [Required]
    public int TypeId { get; set; }

    public Guid? ParentId { get; set; }

    public List<EntityTranslationContentDTO> Translations { get; set; } = new();
}
