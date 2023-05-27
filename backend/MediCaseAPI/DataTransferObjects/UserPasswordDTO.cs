using System.ComponentModel.DataAnnotations;
using MediCaseAPI.Authentication;

namespace MediCaseAPI.DataTransferObjects;

public class UserPasswordDTO
{
    [Required]
    public string CurrentPassword { get; set; } = null!;

    [Required, MinLength(PasswordRules.MinLength), MaxLength(PasswordRules.MaxLength)]
    public string NewPassword { get; set; } = null!;
}
