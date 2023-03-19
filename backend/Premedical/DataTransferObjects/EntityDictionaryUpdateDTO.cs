using System.ComponentModel.DataAnnotations;
using Premedical.Models;

namespace Premedical.DataTransferObjects;

public class EntityDictionaryUpdateDTO
{
    [Required, MaxLength(FieldLengths.Term)]
    public string Term { get; set; } = null!;

    [Required, MaxLength(FieldLengths.Definition)]
    public string Definition { get; set; } = null!;
}
