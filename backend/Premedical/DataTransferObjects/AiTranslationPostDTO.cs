using System.ComponentModel.DataAnnotations;

namespace Premedical.DataTransferObjects;

public class AiTranslationPostDTO
{
    [Required]
    public Guid SourceTranslationId { get; set; }

    [Required]
    public int TargetLangId { get; set; }
}
