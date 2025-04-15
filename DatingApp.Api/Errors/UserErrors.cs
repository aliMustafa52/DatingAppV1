using DatingApp.Api.Abstractions;

namespace DatingApp.Api.Errors
{
    public static class UserErrors
    {
        public static readonly Error UserNotFound =
            new("User.NotFound", "No User was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error FailedToSaveChanges =
        new("Data.FailedToSaveChanges", "Failed to persist changes to the database.", StatusCodes.Status500InternalServerError);
    }
}
