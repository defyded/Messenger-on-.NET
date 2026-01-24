using Messenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Infastucture.Repository
{
    public interface IChatRepository
    {
        Task<Chat?> GetById(Guid Id);
        Task<Chat?> GetByUser(Guid Id);
        Task<ICollection<ChatMessage>> GetChatMessagesByChat(Guid Id);
        Task Delete(Guid Id);
        Task Update(Chat chat);
        Task Add(Chat chat);

    }
}
