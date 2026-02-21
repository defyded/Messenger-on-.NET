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
    public class GroupRepository : IGroupRepository
    {
        private readonly MessengerDBContext _context;
        public GroupRepository(MessengerDBContext context)
        {
            _context = context;
        }
        public Task Add(Group group)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Guid Id)
        {
            Group? tmp = await GetById(Id);
            _context.Groups.Remove(tmp);

        }

        public async Task<Group?> GetById(Guid Id) => await _context.Groups.FirstOrDefaultAsync(x => x.Id == Id);

        public async Task<ICollection<GroupMessage>> GetGroupMessages(Guid Id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == Id);
            
            if (group is null) return new List<GroupMessage>();

            return group.Messages;
        }

        public Task Update(Group group)
        {
            _context.Groups.Update(group);
            return Task.CompletedTask;
        }
    }
}
