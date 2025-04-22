using DatingApp.Api.Abstractions;

namespace DatingApp.Api.Errors
{
    public static class UserErrors
    {
        public static readonly Error UserNotFound =
            new("User.NotFound", "No User was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error FailedToSaveChanges =
        new("Data.FailedToSaveChanges", "Failed to persist changes to the database.", StatusCodes.Status500InternalServerError);

        public static readonly Error PhotoIsNotFound =
            new("User.PhotoIsNotFound", "No Photo was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error MainPhotoNotFound =
            new("User.MainPhotoNotFound", "No Main Photo was found", StatusCodes.Status404NotFound);

        public static readonly Error PhotoAlreadyMain =
            new("User.PhotoAlreadyMain", "This Photo is Already Main photo", StatusCodes.Status400BadRequest);

        public static readonly Error CanootDeleteMainPhoto =
            new("User.CanootDeleteMainPhoto", "Canoot Delete Main Photo", StatusCodes.Status400BadRequest);
    }
}
