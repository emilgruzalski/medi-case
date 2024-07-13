using System.Security.Claims;
using MediCaseAPI.Models;

namespace MediCaseAPI.Services.Interfaces;

public interface ICurrentUserService
{
    /// <summary>Returns the local account of the signed in user, creating it on the first sign in.</summary>
    Task<User> GetOrCreateAsync(ClaimsPrincipal principal);
}
