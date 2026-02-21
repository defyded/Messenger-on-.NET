using Messenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Infastucture.Repository.Interfaces
{
    public interface IUserDevicesRepository
    {
        Task<UserDevice?> GetById(Guid Id);
        //Task<User?> GetByUserDevices(Guid Id);
        Task Add(UserDevice userDevice);
        Task Delete(Guid Id);
    }
}
