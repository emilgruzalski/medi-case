using Premedical.Authentication;
using Premedical.Services.Interfaces;

namespace Premedical.Services;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, PasswordRules.BCryptWorkFactor);

    public bool Verify(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
