using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Messages;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Entities;
using DatingApp.Api.Errors;
using DatingApp.Api.Helpers;
using DatingApp.Api.Interfaces;
using DatingApp.Api.SignalR;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.Api.Services.MessagesService
{
    public class MessagesService(IMessageRepository messageRepository,
        IUserRepository userRepository, IHubContext<PresenceHub> hubContext) 
        : IMessagesService
    {
        private readonly IMessageRepository _messageRepository = messageRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHubContext<PresenceHub> _hubContext = hubContext;

        public async Task<Result<MessageResponse>> CreateMessage(string username,MessageRequest request)
        {
            if (username.Equals(request.RecipientUsername, StringComparison.OrdinalIgnoreCase))
                return Result.Failure<MessageResponse>(MessageErrors.CannotMessageYourself);

            var sender = await _userRepository.GetByUsernameAsync(username);
            var recipient = await _userRepository.GetByUsernameAsync(request.RecipientUsername);
            if(sender is null || recipient is null)
                return Result.Failure<MessageResponse>(UserErrors.UserNotFound);

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = username,
                RecipientUsername = request.RecipientUsername,
                Content = request.Content
            };

            var groupName = MessageHelpers.GetGroupName(username, request.RecipientUsername);
            var group = await _messageRepository.GetMessageGroupAsync(groupName);
            if(group is not null && group.Connections.Any(x => x.Username == request.RecipientUsername))
            {
                message.ReadOn = DateTime.UtcNow;
            }
            else
            {
                var connections = await PresenceTracker.GetConnectionForUser(request.RecipientUsername);
                if(connections is not null && connections.Count != 0)
                {
                    await _hubContext.Clients.Clients(connections).SendAsync("NewMessageReceived",new
                    {
                        username = sender.UserName,
                        knownAs = sender.KnownAs
                    });
                }
            }

            _messageRepository.AddMessage(message);
            var updated = await _messageRepository.SaveChangesAsync();
            if (updated == 0)
                return Result.Failure<MessageResponse>(UserErrors.FailedToSaveChanges);

            var response = message.Adapt<MessageResponse>();

            return Result.Success(response);
        }

        public async Task<PaginatedList<MessageResponse>> GetMessagesForUserAsync(RequestMessageFilters filters)
        {
            var messages = await _messageRepository.GetMessagesForUserAsync(filters);

            return messages;
        }

        public async Task<IEnumerable<MessageResponse>> GetMessageThreadAsync(string currentUsername, string username)
        {
            var messages = await _messageRepository.GetMessagesThreadAsync(currentUsername, username);

            return messages;
        }

        public async Task<Result> DeleteMessageAsync(string username, int messageId)
        {
            var message = await _messageRepository.GetMessageAsync(messageId);
            if(message is null)
                return Result.Failure(MessageErrors.MessageNotFound);

            if(!message.RecipientUsername.Equals(username, StringComparison.OrdinalIgnoreCase) 
                && !message.SenderUsername.Equals(username,StringComparison.OrdinalIgnoreCase))
                return Result.Failure(MessageErrors.MessageNotAllowedToDelete);

            if(message.SenderUsername == username)
                message.SenderDeleted = true;

            if (message.RecipientUsername == username)
                message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted)
                _messageRepository.DeleteMessage(message);

            var updated = await _userRepository.SaveAllAsync();
            if (updated == 0)
                return Result.Failure<UserResponse>(UserErrors.FailedToSaveChanges);

            return Result.Success();
        }
    }
}
