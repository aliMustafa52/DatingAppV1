using DatingApp.Api.Entities;

namespace DatingApp.Api.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(ApplicationUser user);
    }
}
