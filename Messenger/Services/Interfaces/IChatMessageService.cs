using Messenger.Domain.Entities;
using Messenger.DTO;

namespace Messenger.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task DeleteMessageAsync(Guid MessageId, Guid userId);
        Task<ICollection<ChatMessageDto>> GetChatMessagesAsync(Guid chatId, Guid userId);
        Task<ChatMessageDto> SendAsync(CreateChatMessageDto createMessageDto);
    }
}
