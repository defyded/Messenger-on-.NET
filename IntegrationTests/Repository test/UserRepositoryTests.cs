using IntegrationTests;
using Messenger.Domain.Entities;
using Messenger.Infastucture.Repository;


namespace IntegrationTests
{
    //[Collection("Integration")]
    public class UserRepositoryTests : IClassFixture<PostgresTestFixture>
    {
        private readonly PostgresTestFixture _testFixture;
        public UserRepositoryTests(PostgresTestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task Add_User_Then_GetByUsername() //todo
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
            await db.SaveChangesAsync(); //todo
            var loaded = await repo.GetByName("olga");
            Assert.NotNull(loaded);
            Assert.NotEmpty(loaded);
            //Assert.Equal("olga", loaded.Username);
        }
    }
}