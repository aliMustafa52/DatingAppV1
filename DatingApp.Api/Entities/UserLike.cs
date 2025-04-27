namespace DatingApp.Api.Entities
{
    public class UserLike
    {

        public int SourceUserId { get; set; } 
        public ApplicationUser SourceUser { get; set; } = default!;


        public int TargetUserId { get; set; }
        public ApplicationUser TargetUser { get; set; } = default!;
    }
}
