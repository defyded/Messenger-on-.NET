using Messenger.Domain.Entities;

namespace Messenger.DTO
{
    public record ChatDto(Guid ChatId, Guid UserId, Guid CompanionId, bool Blocked, DateTime CreatedAt);
    public record RequestChatCreateDto(Guid CompanionId);
}
