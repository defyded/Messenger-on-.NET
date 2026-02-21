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
    public class UserRepository : IUserRepository
    {
        private readonly MessengerDBContext _context;

        public UserRepository(MessengerDBContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetById(Guid Id) => await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);

        public async Task<User?> GetByName(string username) => await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        public async Task<ICollection<UserDevice>> GetUserDevices(Guid Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user is null) return new List<UserDevice>();

            return user.UserDevices;
        }

        public async Task<List<Chat>> GetChats(Guid Id)
        {
            var user = await _context.Users.Include(x => x.Chats).FirstOrDefaultAsync(x => x.Id == Id);
            
            if (user is null) return new();
            
            return user.Chats;
        }

        public Task Update(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task<ICollection<Group>> GetGroups(Guid Id)
        {
            var user = await _context.Users.Include(x => x.Groups).FirstOrDefaultAsync(x => x.Id == Id);

            if (user is null) return new List<Group>();

            return user.Groups;
        }
    }
}
