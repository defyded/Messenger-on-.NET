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
    public class GroupMessageRepository : IGroupMessageRepository
    {
        private readonly MessengerDBContext _context;

        public GroupMessageRepository(MessengerDBContext context)
        {
            _context = context;
        }

        public async Task Add(GroupMessage groupMessage) => await _context.GroupMessages.AddAsync(groupMessage);

        public async Task<GroupMessage?> GetById(Guid Id) => await _context.GroupMessages.FirstOrDefaultAsync(x => x.Id == Id);
    }
}
