using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Messages;
using DatingApp.Api.Entities;
using DatingApp.Api.Extensions;
using DatingApp.Api.Helpers;
using DatingApp.Api.Interfaces;
using DatingApp.Api.Services.MessagesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.Api.SignalR
{
    [Authorize]
    public class MessageHub(IMessageRepository messageRepository, IMessagesService messagesService) : Hub
    {
        private readonly IMessageRepository _messageRepository = messageRepository;
        private readonly IMessagesService _messagesService = messagesService;

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request.Query["user"];

            if (Context.User is null || string.IsNullOrEmpty(otherUser))
                throw new HubException("Cannot join group");

            var groupName = MessageHelpers.GetGroupName(Context!.User.GetUsername()!, otherUser!);
            await Groups.AddToGroupAsync(Context!.ConnectionId, groupName);

            var group =  await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _messageRepository.GetMessagesThreadAsync(Context?.User.GetUsername()!, otherUser!);
            await Clients.Caller.SendAsync("ReciveMessageThread", messages);
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(MessageRequest request) 
        {
            var username = Context.User!.GetUsername();

            var result = await _messagesService.CreateMessage(username!, request);
            if (result.IsSuccess)
            {
                var groupName = MessageHelpers.GetGroupName(result.Value.SenderUsername, result.Value.RecipientUsername);
                await Clients.Group(groupName).SendAsync("NewMessage", result.Value);
            }
            else
            {
                throw new HubException(result.Error.Description);
            }
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var username = Context.User?.GetUsername() ?? throw new Exception("Cannot get username");
            var group = await _messageRepository.GetMessageGroupAsync(groupName);
            var connection = new Connection { ConnectinId = Context.ConnectionId, Username = username };

            if(group == null)
            {
                group = new Group { Name = groupName };
                _messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            var updated = await _messageRepository.SaveChangesAsync();
            if (updated == 0)
                throw new HubException("Failed to join group");


            return group;
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _messageRepository.GetGroupForConnectionAsync(Context.ConnectionId);
            var connection = group?.Connections.FirstOrDefault(x => x.ConnectinId == Context.ConnectionId);
            if(connection != null && group!= null)
            {
                _messageRepository.RemoveConnection(connection);
                var updated = await _messageRepository.SaveChangesAsync();
                if (updated > 0)
                    return group;
            }
            throw new HubException("Failed to join group");
        }

        
    }
}
