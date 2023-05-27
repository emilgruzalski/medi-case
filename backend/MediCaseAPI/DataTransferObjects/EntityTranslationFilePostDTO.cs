using System.ComponentModel.DataAnnotations;

namespace MediCaseAPI.DataTransferObjects;

public class EntityTranslationFilePostDTO
{
    [Required]
    public Guid TranslationId { get; set; }

    [Required]
    public IFormFile File { get; set; } = null!;
}
