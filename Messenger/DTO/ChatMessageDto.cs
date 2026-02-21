namespace Messenger.DTO
{
    public record ChatMessageDto(
        Guid Id,
        Guid SenderId,
        string Content,
        DateTime CreatedAt,
        DateTime? ReadAt);
    public record CreateChatMessageDto(Guid ChatId, Guid SenderId,string Content);
}
