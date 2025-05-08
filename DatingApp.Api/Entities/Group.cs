namespace DatingApp.Api.Entities
{
    public class Group
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<Connection> Connections { get; set; } = [];
    }
}
