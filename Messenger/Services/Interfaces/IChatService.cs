using Messenger.Domain.Entities;
using Messenger.DTO;

namespace Messenger.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatDto> CreatChatAsync(Guid UserFromId, Guid UserToId);
        Task<IReadOnlyCollection<ChatDto>> GetUserChatsAsync(Guid Id);
        Task DeleteChatAsync(Guid chatId, Guid userId);
    }
}
