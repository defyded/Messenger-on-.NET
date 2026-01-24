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
    public class ReadGroupMessageRepository : IReadGroupMessageRepository
    {
        private readonly MessengerDBContext _context;
        public ReadGroupMessageRepository(MessengerDBContext context)
        {
            _context = context;
        }
        public async Task Add(ReadGroupMessage readGroupMessage) => await _context.ReadGroupMessages.AddAsync(readGroupMessage);

        public async Task<ReadGroupMessage?> GetById(Guid Id) => await _context.ReadGroupMessages.FirstOrDefaultAsync(x => x.Id == Id);

        public Task<List<User>> GetUsersByReadMessage(Guid Id) => _context.ReadGroupMessages.Where(x => x.ReadMessageGroupMessageId == Id)
            .Select(x => x.ReadMessageUser).ToListAsync();
    }
}
