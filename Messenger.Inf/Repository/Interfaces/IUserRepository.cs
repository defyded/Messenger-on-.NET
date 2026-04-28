using Messenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Infastucture.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetById(Guid Id);
        Task<List<User>> GetByName(string username);
        Task<List<Chat>> GetChats(Guid Id);
        Task<ICollection<Group>> GetGroups(Guid Id);
        Task<ICollection<UserDevice>> GetUserDevices(Guid Id);
        Task Update(User user);
        Task Add(User user);

    }
}
