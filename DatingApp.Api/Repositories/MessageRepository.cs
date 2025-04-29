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

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public Task<Message?> GetMessageAsync(int id)
        {
            var message = _context.Messages
                .SingleOrDefaultAsync(x => x.Id == id);

            return message;
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
                .OrderByDescending(x => x.SentOn)
                .ProjectToType<MessageResponse>()
                //.Select(m => new MessageResponse(
                //        m.Id,
                //        m.SenderId,
                //        m.SenderUsername,
                //        m.Sender.Photos.SingleOrDefault(p => p.IsMain)!.Url,
                //        m.RecipientId,
                //        m.RecipientUsername,
                //        m.Recipient.Photos.SingleOrDefault(p => p.IsMain)!.Url,
                //        m.Content,
                //        m.SentOn,
                //        m.ReadOn))
                .ToListAsync();

            //var unreadMessages = messages.Where(x => x.ReadOn == null 
            //        &&  x.RecipientUsername == recipientUsername)
            //        .ToList();
            //if (unreadMessages.Count != 0)
            //{
            //    foreach (var unreadMessage in unreadMessages)
            //    {
            //        unreadMessage.ReadOn = DateTime.UtcNow;
            //        await _context.SaveChangesAsync();
            //    }
            //}
            await _context.Messages
                .Where(m =>
                    m.SenderUsername == currentUsername &&
                    m.RecipientUsername == recipientUsername &&
                    m.ReadOn == null)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(m => m.ReadOn , DateTime.UtcNow));

            return messages.Adapt<IEnumerable<MessageResponse>>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
