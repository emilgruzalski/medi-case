using System.ComponentModel.DataAnnotations;
using MediCaseAPI.Models;

namespace MediCaseAPI.DataTransferObjects;

public class EntityLanguageUpdateDTO
{
    [Required, MaxLength(FieldLengths.LanguageCode)]
    public string LangValue { get; set; } = null!;

    [Required, MaxLength(FieldLengths.Name)]
    public string Name { get; set; } = null!;
}
