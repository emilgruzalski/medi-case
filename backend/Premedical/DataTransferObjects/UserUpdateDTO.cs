using System.ComponentModel.DataAnnotations;
using Premedical.Models;

namespace Premedical.DataTransferObjects;

public class UserUpdateDTO
{
    [Required, EmailAddress, MaxLength(FieldLengths.Email)]
    public string Email { get; set; } = null!;

    [Required, MaxLength(FieldLengths.PersonName)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(FieldLengths.PersonName)]
    public string LastName { get; set; } = null!;
}
