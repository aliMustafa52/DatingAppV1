using DatingApp.Api.Abstractions;

namespace DatingApp.Api.Errors
{
    public static class MessageErrors
    {
        public static readonly Error CannotMessageYourself =
            new("Message.CannotMessageYourself", "Cannot Message Yourself", StatusCodes.Status400BadRequest);

        public static readonly Error MessageNotFound =
            new("Message.NotFound", "Cannot find a Message with this id", StatusCodes.Status404NotFound);

        public static readonly Error MessageNotAllowedToDelete =
            new("Message.NotAllowedToDelete", "You are not allowed to delete this message", StatusCodes.Status400BadRequest);
    }
}
