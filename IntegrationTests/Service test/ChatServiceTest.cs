using Messenger.Domain.Entities;
using Messenger.Infastucture.Repository.Interfaces;
using Messenger.Services;
using Messenger.Services.Interfaces;
using Moq;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Messenger.Services.ChatService;

namespace IntegrationTests.Service_test
{
    public class ChatServiceTest
    {
        private readonly Mock<IChatRepository> _chatRepoMock;
        private readonly IChatService _serviceChat;
        private readonly IUserRepository _userRepo;
        public ChatServiceTest()
        {
            _chatRepoMock = new Mock<IChatRepository>();
            _serviceChat = new ChatService(_chatRepoMock.Object, _userRepo);
        }
        [Fact]
        public async Task Create_Chat_Positive_Case()
        {
            var FromId = Guid.NewGuid();
            var ToId= Guid.NewGuid();

            Chat? savedChat = null;

            _chatRepoMock
                .Setup(r => r.GetByUser(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Chat>());

            

            _chatRepoMock
                .Setup(r => r.Add(It.IsAny<Chat>()))
                .Callback<Chat>(c => {
                    savedChat = c;
                    c.Id = Guid.NewGuid();
                    c.Blocked = false;
                    c.CreatedAt = DateTime.UtcNow;
                    })
                .Returns(Task.CompletedTask);

            var result = await _serviceChat.CreateChatAsync(FromId, ToId);

            Assert.NotNull(result);
            Assert.Equal(ToId, result.CompanionId);
            Assert.False(result.Blocked);

            _chatRepoMock.Verify(
            r => r.Add(It.Is<Chat>(c =>
                c.UserFromId == FromId &&
                c.UserToId == ToId)),
            Times.Once);

            Assert.NotNull(savedChat);
            Assert.Equal(FromId, savedChat.UserFromId);
            Assert.Equal(ToId, savedChat.UserToId);
        }
        [Fact]
        public async Task Try_Create_Existing_Chat()
        {
            var FromId = Guid.NewGuid();
            var ToId = Guid.NewGuid();

            var ExistingChat = new Chat
            {
                Id = Guid.NewGuid(),
                UserFromId = FromId,
                UserToId = ToId,
                Blocked = false,
                CreatedAt = DateTime.UtcNow
            };

            _chatRepoMock
                .Setup(r => r.GetByUser(FromId))
                .ReturnsAsync(new List<Chat> { ExistingChat });

            var result = await _serviceChat.CreateChatAsync(FromId, ToId);

            Assert.NotNull(result);
            Assert.Equal(ExistingChat.Id, result.ChatId);
            Assert.Equal(ToId, result.CompanionId);
            Assert.Equal(ExistingChat.Blocked, result.Blocked);
            Assert.Equal(ExistingChat.CreatedAt, result.CreatedAt);

            _chatRepoMock.Verify(
                r => r.Add(It.IsAny<Chat>()),
                Times.Never);
        }
        [Fact]
        public async Task Try_Delete_Existing_Chat()
        {
            var chatId = Guid.NewGuid();
            var FromId = Guid.NewGuid();
            var ToId = Guid.NewGuid();

            var chat = new Chat { Id = chatId, UserFromId = FromId, UserToId = ToId };

            _chatRepoMock
                .Setup(r => r.GetById(chatId))
                .ReturnsAsync(chat);

            _chatRepoMock
                .Setup(r => r.Delete(chatId))
                .Returns(Task.CompletedTask);

            await _serviceChat.DeleteChatAsync(chatId, FromId);

            _chatRepoMock.Verify(r => r.Delete(chatId), Times.Once);
        }
        [Fact]
        public async Task Try_Delete_NotExisting_Chat()
        {
            var chatId = Guid.NewGuid();
            var UserId = Guid.NewGuid();

            _chatRepoMock
                .Setup(r => r.GetById(chatId))
                .ReturnsAsync((Chat?)null);

            var ex = await Assert.ThrowsAsync<ChatException>(() => 
            _serviceChat.DeleteChatAsync(chatId, UserId));

            Assert.Equal("CHAT_DOES_NOT_EXIST", ex.Code);
        }

        [Fact]
        public async Task Try_Delete_Chat_By_UnauthUser()
        {
            var chatId = Guid.NewGuid();
            var FromId = Guid.NewGuid();
            var ToId = Guid.NewGuid();

            var chat = new Chat { Id = chatId, UserFromId = FromId, UserToId = ToId };

            _chatRepoMock
                .Setup(r => r.GetById(chatId))
                .ReturnsAsync(chat);

            _chatRepoMock
                .Setup(r => r.Delete(chatId))
                .Returns(Task.CompletedTask);

            var ex = await Assert.ThrowsAsync<ChatException>(() =>
            _serviceChat.DeleteChatAsync(chatId, Guid.NewGuid()));

            Assert.Equal("UNAUTHORIZED_ACCESS", ex.Code);
        }
        [Fact]
        public async Task Try_Get_User_Chats()
        {
            var userId = Guid.NewGuid();
            var chat1 = new Chat { Id = Guid.NewGuid(), UserFromId = userId, UserToId = Guid.NewGuid(), Blocked = false, CreatedAt = DateTime.UtcNow };
            var chat2 = new Chat { Id = Guid.NewGuid(), UserFromId = Guid.NewGuid(), UserToId = userId, Blocked = true, CreatedAt = DateTime.UtcNow.AddMinutes(-10) };

            _chatRepoMock
                .Setup(r => r.GetByUser(userId))
                .ReturnsAsync(new List<Chat> { chat1, chat2});
            
            var res = await _serviceChat.GetUserChatsAsync(userId);

            Assert.Equal(2, res.Count);
            Assert.Contains(res, x => x.ChatId == chat1.Id && x.CompanionId == chat1.UserToId);
            Assert.Contains(res, x => x.ChatId == chat2.Id && x.CompanionId == chat2.UserFromId);
        }
    }
}
