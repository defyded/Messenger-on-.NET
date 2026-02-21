using Messenger.Domain.Entities;
using Messenger.DTO;
using Messenger.Hubs;
using Messenger.Infastucture.Repository;
using Messenger.Infastucture.Repository.Interfaces;
using Messenger.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatMessageService(
        IChatMessageRepository chatMessageRepository,
        IChatRepository chatRepository,
        IHubContext<ChatHub> hubContext)
        {
            _chatMessageRepository = chatMessageRepository;
            _chatRepository = chatRepository;
            _hubContext = hubContext;
        }
        public async Task DeleteMessageAsync(Guid MessageId, Guid userId)
        {
            var message = await _chatMessageRepository.GetById(MessageId);
            if (message == null)
                throw new ChatMessageException("MESSAGE_NOT_FOUND", "message not found");

            if (message.SenderId != userId)
                throw new ChatMessageException("UNAUTHORIZED_USER", "unauthorized user");
            message.Deleted = true;
            await _chatMessageRepository.Update(message);

            await _hubContext.Clients.Group(message.ChatId.ToString())
                .SendAsync("DeleteMessage", MessageId);
        }

        public async Task<ICollection<ChatMessageDto>> GetChatMessagesAsync(Guid chatId, Guid userId)
        {
            var chat = await _chatRepository.GetById(chatId);
            
            if (chat == null)
                throw new ChatMessageException("CHAT_NOT_FOUND", "chat not found");

            if (chat.UserFromId != userId && chat.UserToId != userId)
                throw new ChatMessageException("USER_IS_NOT_PART_OF_CHAT", "user is not part of this chat");

            var messages = await _chatMessageRepository.GetByChatId(chatId);

            return messages
                .Where(m => !m.Deleted)
                .Select(m => new ChatMessageDto
                (
                    m.Id,
                    m.SenderId,
                    m.Content,
                    m.CreatedAt,
                    m.ReadAt
                ))
                .ToList()
                .AsReadOnly();
        }

        public async Task<ChatMessageDto> SendAsync(CreateChatMessageDto createMessageDto)
        {
            var chat = await _chatRepository.GetById(createMessageDto.ChatId);

            if (chat == null)
                throw new ChatMessageException("CHAT_NOT_FOUND", "chat not found");

            if(chat.UserFromId != createMessageDto.SenderId || chat.UserToId != createMessageDto.SenderId)
                throw new ChatMessageException("USER_IS_NOT_PART_OF_CHAT", "user is not part of this chat");

            var message = new ChatMessage
            {
                ChatId = createMessageDto.ChatId,
                SenderId = createMessageDto.SenderId,
                Content = createMessageDto.Content,
                CreatedAt = DateTime.UtcNow,
                Deleted = false
            };

            await _chatMessageRepository.Add(message);

            var messageDto = new ChatMessageDto
                (
                    message.Id,
                    message.SenderId,
                    message.Content,
                    message.CreatedAt,
                    message.ReadAt
                );
            await _hubContext.Clients.Group(createMessageDto.ChatId.ToString())
                .SendAsync("ReceiveMessage", messageDto);

            return messageDto;
        }
    }
    public sealed class ChatMessageException : Exception
    {
        public string Code { get; }

        public ChatMessageException(string code, string message) : base(message)
            => Code = code;
    }
}
