using Messenger.Domain.Entities;
using Messenger.Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

        public async Task Add(ChatMessage chatMessage) => await _context.ChatMessages.AddAsync(chatMessage);

        public async Task<ChatMessage?> GetById(Guid Id) => await _context.ChatMessages.FirstOrDefaultAsync(x => x.Id == Id);
        //Можно добавить удаление Delete 
    }
}
