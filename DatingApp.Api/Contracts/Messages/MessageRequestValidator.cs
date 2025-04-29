using FluentValidation;

namespace DatingApp.Api.Contracts.Messages
{
    public class MessageRequestValidator : AbstractValidator<MessageRequest>
    {
        public MessageRequestValidator()
        {
            RuleFor(x => x.RecipientUsername).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
