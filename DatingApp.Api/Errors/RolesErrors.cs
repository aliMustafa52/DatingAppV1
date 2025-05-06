using DatingApp.Api.Abstractions;

namespace DatingApp.Api.Errors
{
    public static class RolesErrors
    {
        public static readonly Error RoleNotFound =
            new("Role.NotFound", "No Role was found this name", StatusCodes.Status404NotFound);
    }
}
