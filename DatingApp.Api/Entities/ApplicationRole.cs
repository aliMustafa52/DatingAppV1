using Microsoft.AspNetCore.Identity;

namespace DatingApp.Api.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {

        public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    }
}
