using DatingApp.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.Api.SignalR
{
    [Authorize]
    public class PresenceHub(IPresenceTracker presenceTracker) : Hub
    {
        private readonly IPresenceTracker _presenceTracker = presenceTracker;

        public override async Task OnConnectedAsync()
        {
            if (Context.User == null)
                throw new HubException("Cannot get cuurent user");

            var isOnline =  await _presenceTracker.UserConnected(Context.User.GetUsername()!, Context.ConnectionId);
            if(isOnline)
                await Clients.Others.SendAsync("UserIsOnline", Context.User?.GetUsername());

            var cuuretnUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", cuuretnUsers);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if(Context.User == null)
                throw new HubException("Cannot get cuurent user");

            var isOffline = await _presenceTracker.UserDisconnected(Context.User.GetUsername()!, Context.ConnectionId);
            if(isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetUsername());


            await base.OnDisconnectedAsync(exception);
        }
    }
}
