using System.ComponentModel.DataAnnotations;
using MediCaseAPI.Models;

namespace MediCaseAPI.DataTransferObjects;

public class UserAdminUpdateDTO
{
    [Required, EmailAddress, MaxLength(FieldLengths.Email)]
    public string Email { get; set; } = null!;

    [Required, MaxLength(FieldLengths.PersonName)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(FieldLengths.PersonName)]
    public string LastName { get; set; } = null!;

    [Required, MaxLength(FieldLengths.Role)]
    public string Role { get; set; } = null!;

    public List<int> GroupIds { get; set; } = new();
}
