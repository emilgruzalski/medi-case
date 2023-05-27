using System.ComponentModel.DataAnnotations;

namespace MediCaseAPI.DataTransferObjects;

public class EntityPostDTO
{
    [Required]
    public int TypeId { get; set; }

    public Guid? ParentId { get; set; }

    public int? Order { get; set; }

    public bool IsCorrect { get; set; }

    public List<EntityTranslationContentDTO> Translations { get; set; } = new();
}
