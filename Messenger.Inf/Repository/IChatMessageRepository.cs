using Messenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Infastucture.Repository
{
    public interface IChatMessageRepository
    {
        Task<ChatMessage?> GetById(Guid Id);
        Task Add(ChatMessage chatMessage);
    }
}
