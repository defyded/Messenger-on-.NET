using Messenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Infastucture.Repository
{
    public interface IReadGroupMessageRepository
    {
        Task<ReadGroupMessage?> GetById(Guid Id);
        Task Add(ReadGroupMessage readGroupMessage);
        Task<List<User>> GetUsersByReadMessage(Guid Id);
        
    }
}
