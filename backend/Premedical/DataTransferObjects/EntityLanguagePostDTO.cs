using System.ComponentModel.DataAnnotations;
using Premedical.Models;

namespace Premedical.DataTransferObjects;

public class EntityLanguagePostDTO
{
    [Required, MaxLength(FieldLengths.LanguageCode)]
    public string LangValue { get; set; } = null!;

    [Required, MaxLength(FieldLengths.Name)]
    public string Name { get; set; } = null!;
}
