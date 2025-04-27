using DatingApp.Api.Abstractions;

namespace DatingApp.Api.Errors
{
    public static class LikeErrors
    {
        public static readonly Error CannotLikeYourself =
            new("CannotLikeYourself", "You cannot like yourself", StatusCodes.Status400BadRequest);
    }
}
