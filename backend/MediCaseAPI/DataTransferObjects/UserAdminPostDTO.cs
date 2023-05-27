using System.ComponentModel.DataAnnotations;
using MediCaseAPI.Authentication;

namespace MediCaseAPI.DataTransferObjects;

public class UserAdminPostDTO : UserAdminUpdateDTO
{
    [Required, MinLength(PasswordRules.MinLength), MaxLength(PasswordRules.MaxLength)]
    public string Password { get; set; } = null!;
}
