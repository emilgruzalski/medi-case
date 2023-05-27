using System.ComponentModel.DataAnnotations;
using MediCaseAPI.Models;

namespace MediCaseAPI.DataTransferObjects;

public class EntityTranslationContentDTO
{
    [Required]
    public int LangId { get; set; }

    [Required, MaxLength(FieldLengths.Title)]
    public string Title { get; set; } = null!;

    public string? Content { get; set; }
}
