using System.ComponentModel.DataAnnotations;
using MediCaseAPI.Models;

namespace MediCaseAPI.DataTransferObjects;

public class GroupPostDTO
{
    [Required, MaxLength(FieldLengths.GroupName)]
    public string Name { get; set; } = null!;

    [MaxLength(FieldLengths.GroupDescription)]
    public string? Description { get; set; }
}
