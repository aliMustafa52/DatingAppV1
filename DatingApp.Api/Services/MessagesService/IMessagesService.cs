using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Messages;

namespace DatingApp.Api.Services.MessagesService
{
    public interface IMessagesService
    {
        Task<Result<MessageResponse>> CreateMessage(string username, MessageRequest request);

        Task<PaginatedList<MessageResponse>> GetMessagesForUserAsync(RequestMessageFilters filters);

        Task<IEnumerable<MessageResponse>> GetMessageThreadAsync(string currentUsername, string username);

        Task<Result> DeleteMessageAsync(string username, int messageId);
    }
}
