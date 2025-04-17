using FluentValidation;

namespace DatingApp.Api.Contracts.Users
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Introduction)
                .NotEmpty();

            RuleFor(x => x.Interests)
                .NotEmpty();

            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotEmpty();
        }
    }
}
