using Messenger.Domain.Entities;
using Messenger.Infastructure.Persistence;
using Messenger.Infastucture.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Infastucture.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly MessengerDBContext _context;

        public ChatRepository(MessengerDBContext context)
        {
            _context = context;
        }

        public async Task Add(Chat chat) 
        { 
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
        }


        public async Task Delete(Guid ChatId)
        {
            await _context.Chats
                .Where(x => x.Id == ChatId)
                .ExecuteDeleteAsync();
        }


        public async Task<Chat?> GetById(Guid ChatId) => await _context.Chats.FirstOrDefaultAsync(x => x.Id == ChatId);

        public async Task<ICollection<Chat>> GetByUser(Guid UserId)//todo поменять название параметра
        { 
            return await _context.Chats.Where(x => x.UserFromId == UserId || x.UserToId == UserId)
                .ToListAsync(); 
        }

        public async Task<ICollection<ChatMessage>> GetChatMessagesByChat(Guid ChatId)//todo поменять название параметра
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(x => x.Id == ChatId);

            if (chat is null) return new List<ChatMessage>();

            return chat.Messages;
        }

        public async Task Update(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
        }
    }
}
