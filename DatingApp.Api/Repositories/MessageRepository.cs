using CloudinaryDotNet.Actions;
using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Messages;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using DatingApp.Api.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Repositories
{
    public class MessageRepository(ApplicationDbContext context) : IMessageRepository
    {
        private readonly ApplicationDbContext _context = context;

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection?> GetConnectionAsync(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group?> GetGroupForConnectionAsync(string connectionId)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .Where(x => x.Connections.Any(x => x.ConnectinId == connectionId))
                .FirstOrDefaultAsync();
        }

        public Task<Message?> GetMessageAsync(int id)
        {
            var message = _context.Messages
                .SingleOrDefaultAsync(x => x.Id == id);

            return message;
        }

        public async Task<Group?> GetMessageGroupAsync(string groupName)
        {
            return await _context.Groups
                    .Include(x => x.Connections)
                    .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<PaginatedList<MessageResponse>> GetMessagesForUserAsync(RequestMessageFilters filters)
        {
            var query = _context.Messages
                            .OrderByDescending(x => x.SentOn)
                            .AsQueryable();

            query = filters.Container switch
            {
                "Inbox" => query.Where(x => x.RecipientUsername == filters.Username && !x.RecipientDeleted ),
                "Outbox" => query.Where(x => x.SenderUsername == filters.Username && !x.SenderDeleted),
                _ => query.Where(x => x.RecipientUsername == filters.Username
                                            && x.ReadOn == null && !x.RecipientDeleted),
            };

            var messages = query.ProjectToType<MessageResponse>();

            var response = await PaginatedList<MessageResponse>.CreateAsync(messages
                    , filters.PageNumber, filters.PageSize);

            return response;
        }

        public async Task<IEnumerable<MessageResponse>> GetMessagesThreadAsync(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messages
                .AsNoTracking()
                .Where(m => 
                (m.SenderUsername == currentUsername && m.RecipientUsername == recipientUsername 
                    && !m.SenderDeleted) 
                ||(m.SenderUsername == recipientUsername && m.RecipientUsername == currentUsername 
                    && !m.RecipientDeleted))
                .OrderBy(x => x.SentOn)
                .ProjectToType<MessageResponse>()

                .ToListAsync();

            await _context.Messages
                .Where(m =>
                    m.SenderUsername == currentUsername &&
                    m.RecipientUsername == recipientUsername &&
                    m.ReadOn == null)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(m => m.ReadOn , DateTime.UtcNow));

            return messages;
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
