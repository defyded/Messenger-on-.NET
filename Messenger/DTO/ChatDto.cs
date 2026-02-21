using Messenger.Domain.Entities;

namespace Messenger.DTO
{
    public record ChatDto(Guid Id, Guid CompanionId, bool Blocked, DateTime CreatedAt);
}
