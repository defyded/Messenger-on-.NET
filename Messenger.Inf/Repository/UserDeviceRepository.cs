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
    public class UserDeviceRepository : IUserDevicesRepository
    {
        private readonly MessengerDBContext _context;

        public UserDeviceRepository(MessengerDBContext context)
        {
            _context = context;
        }

        public async Task Add(UserDevice userDevice) => await _context.UserDevices.AddAsync(userDevice);

        public async Task Delete(Guid Id)
        {
            UserDevice? tmp = await GetById(Id);
            if (tmp is null) return;
            _context.UserDevices.Remove(tmp);
        }

        public async Task<UserDevice?> GetById(Guid Id) => await _context.UserDevices.FirstOrDefaultAsync(x => x.Id == Id);

        //public async Task<User?> GetByUserDevices(Guid Id) => await _context.UserDevices.FirstOrDefaultAsync(x => x.Id == Id).Result.User;
    }
}
