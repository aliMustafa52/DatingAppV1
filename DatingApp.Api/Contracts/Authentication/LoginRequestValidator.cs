using FluentValidation;

namespace DatingApp.Api.Contracts.Authentication
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty();

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
