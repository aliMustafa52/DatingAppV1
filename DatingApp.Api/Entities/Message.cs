namespace DatingApp.Api.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderUsername { get; set; } = string.Empty;
        public string RecipientUsername { get; set; } = string.Empty;

        public string Content { get; set;} = string.Empty;

        public DateTime SentOn { get; set;} = DateTime.UtcNow;
        public DateTime? ReadOn { get; set;}

        public bool SenderDeleted { get; set;}
        public bool RecipientDeleted { get; set;}


        public int SenderId { get; set; }
        public int RecipientId { get; set; }

        // Navigation Properies
        public ApplicationUser Sender { get; set; } = default!;
        public ApplicationUser Recipient { get; set; } = default!;
    }
}
