using Messenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = Messenger.Domain.Entities.Group;

namespace Messenger.Infastucture.Repository
{
    public interface IGroupRepository
    {
        Task<Group?> GetById(Guid Id);
        Task<ICollection<GroupMessage>> GetGroupMessages(Guid Id);
        Task Add(Group group);
        Task Update(Group group);
        Task Delete (Guid Id);
    }
}
