using System.ComponentModel.DataAnnotations;
using MediCaseAPI.Models;

namespace MediCaseAPI.DataTransferObjects;

public class EntityDictionaryPostDTO
{
    [Required]
    public Guid EntityId { get; set; }

    [Required]
    public int LangId { get; set; }

    [Required, MaxLength(FieldLengths.Term)]
    public string Term { get; set; } = null!;

    [Required, MaxLength(FieldLengths.Definition)]
    public string Definition { get; set; } = null!;
}
