using System.ComponentModel.DataAnnotations;

namespace Premedical.DataTransferObjects;

public class EntityTranslationFilePostDTO
{
    [Required]
    public Guid TranslationId { get; set; }

    [Required]
    public IFormFile File { get; set; } = null!;
}
