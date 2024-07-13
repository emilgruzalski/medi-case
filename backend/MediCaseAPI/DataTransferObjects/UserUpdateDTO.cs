using System.ComponentModel.DataAnnotations;
using MediCaseAPI.Models;

namespace MediCaseAPI.DataTransferObjects;

public class UserUpdateDTO
{
    [Required, MaxLength(FieldLengths.PersonName)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(FieldLengths.PersonName)]
    public string LastName { get; set; } = null!;
}
