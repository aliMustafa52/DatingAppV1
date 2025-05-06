using FluentValidation;

namespace DatingApp.Api.Contracts.Users
{
    public class RoleRequestValidator : AbstractValidator<RoleRequest>
    {
        public RoleRequestValidator()
        {
            RuleFor(x => x.Roles)
                .NotEmpty()
                .WithMessage("You must select at least one role");
        }
    }
}
