using System.Security.Claims;

namespace DatingApp.Api.Extensions
{
    public static class UserExtensions
    {
        public static string? GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email);
        }
    }
}
