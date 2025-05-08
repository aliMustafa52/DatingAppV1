namespace DatingApp.Api.SignalR
{
    public interface IPresenceTracker
    {
        Task<bool> UserConnected(string username, string connectionId);

        Task<bool> UserDisconnected(string username, string connectionId);

        Task<string[]> GetOnlineUsers();
    }
}
