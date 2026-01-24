using Messenger.Domain.Entities;
using Messenger.Infastucture.Repository;

namespace IntegrationTests
{
    public class UserRepositoryTests : IClassFixture<PostgresTestFixture>
    {
        private readonly PostgresTestFixture _testFixture;
        public UserRepositoryTests(PostgresTestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async void Add_User_Then_GetByUsername()
        {
            var dbName = _testFixture.PgDb;
            await using var db = await _testFixture.CreateDbContextAsync(dbName);
            var repo = new UserRepository(db);
            var user = new User
            {
                Username = "olga",
                Email = "olga@test.com",
                PasswordHash = "hash",
                Deleted = false,
                LastSeenAt = DateTime.UtcNow
            };
            await repo.Add(user);
            var loaded = await repo.GetByName("olga");
            Assert.NotNull(loaded);
            Assert.Equal(user.Id, loaded!.Id);
            Assert.Equal("olga", loaded.Username);
        }
    }
}