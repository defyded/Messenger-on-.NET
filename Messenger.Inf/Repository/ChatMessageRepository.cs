using Messenger.Domain.Entities;
using Messenger.Infastructure.Persistence;
using Messenger.Infastucture.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Infastucture.Repository
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly MessengerDBContext _context;

        public ChatMessageRepository(MessengerDBContext context)
        {
            _context = context;
        }

        public async Task Add(ChatMessage chatMessage)
        {
            await _context.ChatMessages.AddAsync(chatMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<ChatMessage>> GetByChatId(Guid chatId)
        {
            return await _context.ChatMessages
                .Where(x => x.ChatId == chatId)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<ChatMessage?> GetById(Guid MessageId) => await _context.ChatMessages.SingleOrDefaultAsync(x => x.Id == MessageId);

        public async Task Update(ChatMessage chatMessage)
        {
            _context.ChatMessages.Update(chatMessage);
            await _context.SaveChangesAsync();
        }
    }
}
