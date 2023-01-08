using System.ComponentModel.DataAnnotations;
using Premedical.Authentication;

namespace Premedical.DataTransferObjects;

public class UserPasswordDTO
{
    [Required]
    public string CurrentPassword { get; set; } = null!;

    [Required, MinLength(PasswordRules.MinLength), MaxLength(PasswordRules.MaxLength)]
    public string NewPassword { get; set; } = null!;
}
