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
        private readonly IUserRepository _userRepository;
        public ChatService(IChatRepository chatRepository, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        public async Task<ChatDto> CreateChatAsync(Guid UserFromId, Guid UserToId)
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
                var companion = existingChat.UserFromId == UserFromId ? existingChat.UserTo : existingChat.UserFrom;
                var lastMessage = existingChat.Messages?.OrderByDescending(x => x.CreatedAt).FirstOrDefault();

                var lastMessageDto = lastMessage != null ? new ChatMessageDto
                    (
                        lastMessage.Id,
                        lastMessage.SenderId,
                        lastMessage.Content,
                        lastMessage.CreatedAt,
                        lastMessage.ReadAt
                    ) : null;
                return new ChatDto(
                    existingChat.Id,//todo достать из репа юзера его имя оп toid|| upd1 здесь так же
                    companion.Id,
                    companion.Username,
                    companion.AvatarUrl,
                    lastMessageDto,
                    existingChat.Blocked,
                    existingChat.CreatedAt
                );
            }
            var userTo = await _userRepository.GetById(UserToId);
            if (userTo == null) throw new ChatException("USER_NOT_FOUND", "user not found");

            var chat = new Chat
            {
                UserFromId = UserFromId,
                UserToId = UserToId,
                CreatedAt = DateTime.UtcNow,
                Blocked = false
            };
            await _chatRepository.Add(chat);
            return new ChatDto(
                chat.Id,
                userTo.Id,
                userTo.Username,//todo достать из репа юзера его имя оп toid|| upd1 я подумал что достать имя пользователя можно по чату, тк у чата есть UserTo
                userTo.AvatarUrl,
                null,
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
        
        //также обновио получение чатов пользователем
        public async Task<IReadOnlyCollection<ChatDto>> GetUserChatsAsync(Guid userId)
        {
            var chats = await _chatRepository.GetByUser(userId);
            return chats.Select(x =>
            {
                var companion = x.UserFromId == userId ? x.UserTo : x.UserFrom;
                var lastMessage = x.Messages
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefault();
                var lastMessageDto = lastMessage != null ? new ChatMessageDto(
                        lastMessage.Id,
                        lastMessage.SenderId,
                        lastMessage.Content,
                        lastMessage.CreatedAt,
                        lastMessage.ReadAt
                ) : null;
                return new ChatDto(
                    x.Id,
                    companion.Id,
                    companion.Username,
                    companion.AvatarUrl,
                    lastMessageDto,
                    x.Blocked,
                    x.CreatedAt);
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
        }

        public sealed class ChatException : Exception
        {
            public string Code { get; }

            public ChatException(string code, string message) : base(message)
                => Code = code;
        }
    }
}
