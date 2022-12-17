using System.ComponentModel.DataAnnotations;
using Premedical.Authentication;
using Premedical.Models;

namespace Premedical.DataTransferObjects;

public class UserRegisterDTO
{
    [Required, EmailAddress, MaxLength(FieldLengths.Email)]
    public string Email { get; set; } = null!;

    [Required, MinLength(PasswordRules.MinLength), MaxLength(PasswordRules.MaxLength)]
    public string Password { get; set; } = null!;

    [Required, MaxLength(FieldLengths.PersonName)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(FieldLengths.PersonName)]
    public string LastName { get; set; } = null!;
}
