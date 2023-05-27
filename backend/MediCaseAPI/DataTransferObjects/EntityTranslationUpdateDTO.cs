using System.ComponentModel.DataAnnotations;
using MediCaseAPI.Models;

namespace MediCaseAPI.DataTransferObjects;

public class EntityTranslationUpdateDTO
{
    [Required, MaxLength(FieldLengths.Title)]
    public string Title { get; set; } = null!;

    public string? Content { get; set; }
}
