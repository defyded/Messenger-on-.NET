using Messenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Infastucture.Repository.Interfaces
{
    public interface IChatMessageRepository
    {
        Task<ChatMessage?> GetById(Guid MessageId);
        Task<ICollection<ChatMessage>> GetByChatId(Guid chatId);
        Task Add(ChatMessage chatMessage);
        Task Update(ChatMessage chatMessage);
    }
}
