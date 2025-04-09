using FluentValidation;

namespace DatingApp.Api.Contracts.Authentication
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty();

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
