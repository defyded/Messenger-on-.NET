using Messenger.Domain.Entities;

namespace Messenger.DTO
{
    public record ChatDto(
        Guid ChatId, 
        Guid CompanionId,
        string CompanionUsername,
        string? AvatarURL, 
        ChatMessageDto? LastMessage,  
        bool Blocked, 
        DateTime CreatedAt);
    public record RequestChatCreateDto(Guid CompanionId);
}
