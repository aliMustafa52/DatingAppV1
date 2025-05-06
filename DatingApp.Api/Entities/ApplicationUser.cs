using Microsoft.AspNetCore.Identity;

namespace DatingApp.Api.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateOnly DateOfBirth { get; set; }
        public required string KnownAs { get; set; }
        public DateTime Created {  get; set; } = DateTime.UtcNow;
        public DateTime LastActive {  get; set; } = DateTime.UtcNow;

        public required string Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }

        public ICollection<Photo> Photos { get; set; } = [];

        //public List<ApplicationUser> LikedUsers { get; set; } = [];
        //public List<ApplicationUser> LikedByUsers { get; set; } = [];

        public List<UserLike> LikedUsers { get; set; } = [];
        public List<UserLike> LikedByUsers { get; set; } = [];

        public List<Message> SentMessages { get; set; } = [];
        public List<Message> RecivedMessages { get; set; } = [];

        public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    }
}
