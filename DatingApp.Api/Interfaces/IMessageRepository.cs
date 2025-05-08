using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Messages;
using DatingApp.Api.Entities;

namespace DatingApp.Api.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);

        void DeleteMessage(Message message);

        Task<Message?> GetMessageAsync(int id);

        Task<PaginatedList<MessageResponse>> GetMessagesForUserAsync(RequestMessageFilters filters);

        Task<IEnumerable<MessageResponse>> GetMessagesThreadAsync(string currentUsername, string recipientUsername);

        Task<int> SaveChangesAsync();

        void AddGroup(Group group);
        void RemoveConnection(Connection connection);

        Task<Connection?> GetConnectionAsync(string connectionId);
        Task<Group?> GetMessageGroupAsync(string groupName);
        Task<Group?> GetGroupForConnectionAsync(string connectionId);
    }
}