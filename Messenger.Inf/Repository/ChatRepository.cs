using Messenger.Domain.Entities;
using Messenger.Infastructure.Persistence;
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

        public async Task Add(Chat chat) => await _context.Chats.AddAsync(chat);


        public async Task Delete(Guid Id)
        {
            Chat? tmp = await GetById(Id);
            //добавить проверку
             _context.Chats.Remove(tmp);
        }

        public async Task<Chat?> GetById(Guid Id) => await _context.Chats.FirstOrDefaultAsync(x => x.Id == Id);

        public async Task<Chat?> GetByUser(Guid Id) => await _context.Chats.FirstOrDefaultAsync(x => x.UserFromId == Id || x.UserToId == Id);

        public async Task<ICollection<ChatMessage>> GetChatMessagesByChat(Guid Id)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(x => x.Id == Id);

            if (chat is null) return new List<ChatMessage>();

            return chat.Messages;
        }

        public Task Update(Chat chat)
        {
            _context.Chats.Update(chat);
            return Task.CompletedTask;
        }
    }
}
