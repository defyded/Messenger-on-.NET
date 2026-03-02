using Messenger.Domain.Entities;
using Messenger.DTO;
using Messenger.Infastructure.Persistence;
using Messenger.Infastucture.Repository;
using Messenger.Infastucture.Repository.Interfaces;
using Messenger.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<ChatDto> CreatChatAsync(Guid UserFromId, Guid UserToId)
        {
            var chats = await _chatRepository.GetByUser(UserFromId);

            var existingChat = chats.FirstOrDefault(c => 
                (c.UserFromId == UserFromId && c.UserToId == UserToId) || 
                (c.UserFromId == UserToId && c.UserToId == UserFromId));

            if (UserFromId == UserToId)
            {
                throw new ChatException("CAN_NOT_CREATE_CHAT_WTIH_MYSELF", "can not create chat with myself");
            }
            if (existingChat != null)
            {
                return new ChatDto(
                    existingChat.Id,
                    UserFromId,
                    UserToId,
                    existingChat.Blocked,
                    existingChat.CreatedAt
                );
            }

            var chat = new Chat
            {
                UserFromId = UserFromId,
                UserToId = UserToId,
                CreatedAt = DateTime.UtcNow
            };
            await _chatRepository.Add(chat);
            return new ChatDto(
                chat.Id,
                UserFromId,
                UserToId,
                chat.Blocked,
                chat.CreatedAt
            );
        }

        public async Task DeleteChatAsync(Guid chatId, Guid userId)
        {
            var chat = await _chatRepository.GetById(chatId);
            if (chat == null)
            {
                throw new ChatException("CHAT_DOES_NOT_EXIST", "chat does not exist");
            }

            if (chat.UserFromId != userId && chat.UserToId != userId)
            {
                throw new ChatException("UNAUTHORIZED_ACCESS", "unauthorized access");
            }
            await _chatRepository.Delete(chatId);
        }
        

        public async Task<IReadOnlyCollection<ChatDto>> GetUserChatsAsync(Guid userId)
        {
            var chats = await _chatRepository.GetByUser(userId);

            return chats.Select(x => new ChatDto(
                x.Id,
                userId,
                x.UserFromId == userId ? x.UserToId : x.UserFromId,
                x.Blocked,
                x.CreatedAt
            )).ToList();
        }

        public sealed class ChatException : Exception
        {
            public string Code { get; }

            public ChatException(string code, string message) : base(message)
                => Code = code;
        }
    }
}
