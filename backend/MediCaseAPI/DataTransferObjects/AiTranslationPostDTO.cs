using System.ComponentModel.DataAnnotations;

namespace MediCaseAPI.DataTransferObjects;

public class AiTranslationPostDTO
{
    [Required]
    public Guid SourceTranslationId { get; set; }

    [Required]
    public int TargetLangId { get; set; }
}
