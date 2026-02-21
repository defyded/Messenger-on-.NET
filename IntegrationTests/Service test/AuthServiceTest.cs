using Messenger.Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Messenger.Services;
using Messenger.Services.Interfaces;
using Messenger.Domain.Entities;
using Messenger.DTO;

namespace IntegrationTests.Service_test
{
    public sealed class AuthServiceTest
    {
        private static MessengerDBContext CreateDb(string dbName)
        {
            var opt = new DbContextOptionsBuilder<MessengerDBContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new MessengerDBContext(opt);
        }
        private static (AuthService svc, MessengerDBContext db, Mock<ITokenService> tokens) CreateSut(string dbName)
        {
            var db = CreateDb(dbName);
            var tokens = new Mock<ITokenService>(MockBehavior.Strict);
            tokens
                .Setup(x => x.CreateAccesToken(It.IsAny<User>()))
                .Returns(("TEST_TOKEN", new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
            var svc = new AuthService(db, tokens.Object);

            return (svc, db, tokens);
        }
        [Fact]
        public async Task RegisterAsync_creates_user_normalizes_username_hashes_password_and_returns_token()
        {
            var (svc, db, tokens) = CreateSut(nameof(RegisterAsync_creates_user_normalizes_username_hashes_password_and_returns_token));
            var res = await svc.RegisterAsync(new RegisterRequest("Alice", "alice12345@gmail.com", "passwordQWERTY"));
            res.Username.Should().Be("alice");
            res.AccsesToken.Should().Be("TEST_TOKEN");
            res.ExpiresAtUtc.Should().Be(new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            var user = await db.Users.SingleAsync();
            user.Username.Should().Be("alice");
            user.PasswordHash.Should().NotBeNullOrWhiteSpace();
            user.PasswordHash.Should().NotBe("passwordQWERTY");
            tokens.Verify(x => x.CreateAccesToken(It.Is<User>(u => u.Id == user.Id && u.Username == "alice")), Times.Once());
        }
        [Fact]
        public async Task RegisterAsync_throws_then_username_taken()
        {
            var (svc, db, token) = CreateSut(nameof(RegisterAsync_throws_then_username_taken));
            db.Users.Add(new User
            {
                Username = "bob",
                Email = "any@gmail.com",
                PasswordHash = "qwerty12345"
            });
            await db.SaveChangesAsync();
            var act = () => svc.RegisterAsync(new RegisterRequest("BOB", "any2@gmail.com", "qwerty12345"));
            var ex = await Assert.ThrowsAsync<AuthException>(act);
            ex.Code.Should().Be("USERNAME_OR_EMAIL_TAKEN");
        }
        [Fact]
        public async Task RegisterAsync_throws_then_email_taken()
        {
            var (svc, db, token) = CreateSut(nameof(RegisterAsync_throws_then_username_taken));
            db.Users.Add(new User
            {
                Username = "anyname1",
                Email = "any@gmail.com",
                PasswordHash = "qwerty12345"
            });
            await db.SaveChangesAsync();
            var act = () => svc.RegisterAsync(new RegisterRequest("anyname1", "any@gmail.com", "qwerty12345"));
            var ex = await Assert.ThrowsAsync<AuthException>(act);
            ex.Code.Should().Be("USERNAME_OR_EMAIL_TAKEN");
        }
    }
}
