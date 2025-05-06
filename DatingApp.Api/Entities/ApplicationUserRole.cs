using Microsoft.AspNetCore.Identity;

namespace DatingApp.Api.Entities
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public ApplicationUser User { get; set; } = default!;
        public ApplicationRole Role { get; set; } = default!;
    }
}
