using System.ComponentModel.DataAnnotations;
using Premedical.Authentication;

namespace Premedical.DataTransferObjects;

public class UserAdminPostDTO : UserAdminUpdateDTO
{
    [Required, MinLength(PasswordRules.MinLength), MaxLength(PasswordRules.MaxLength)]
    public string Password { get; set; } = null!;
}
