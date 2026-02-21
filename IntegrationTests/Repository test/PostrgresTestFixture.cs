using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using IntegrationTests;
using Messenger.Domain.Entities;
using Messenger.Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    //[CollectionDefinition("Integration", DisableParallelization = true)]
    public sealed class PostgresTestFixture : IAsyncLifetime
    {
        private DotNet.Testcontainers.Containers.IContainer _container = null!;
        private string _adminConnectionString = null!;
        private const string PgUser = "thelowestuser_messenger";
        private const string PgPassword = "123456789";
        public string PgDb { get; private set; } = "MessengerDB";

        [Obsolete]
        public async Task InitializeAsync()
        {
            _container = new ContainerBuilder()
                .WithImage("postgres:16-alpine")
                .WithEnvironment("POSTGRES_USER", PgUser)
                .WithEnvironment("POSTGRES_PASSWORD", PgPassword)
                .WithEnvironment("POSTGRES_DB", PgDb)
                .WithPortBinding(5432, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("database system is ready to accept connections"))
                .Build();
            await _container.StartAsync();
            var host = _container.Hostname;
            var port = _container.GetMappedPublicPort(5432);
            _adminConnectionString =
                $"Host={host};Port={port};Database={PgDb};Username={PgUser};Password={PgPassword};Pooling=false";
        }
        public async Task DisposeAsync()
        {
            await _container.StopAsync();
            await _container.DisposeAsync();
        }

        public async Task<MessengerDBContext> CreateDbContextAsync(string databaseName, CancellationToken ct = default)
        {
            await CreateDatabaseAsync(databaseName, ct);
            var cs =
    $"Host={_container.Hostname};Port={_container.GetMappedPublicPort(5432)};Database={databaseName};Username={PgUser};Password={PgPassword};Pooling=false";
            var options = new DbContextOptionsBuilder<MessengerDBContext>()
                .UseNpgsql(cs)
                .EnableSensitiveDataLogging()
                .Options;
            var db = new MessengerDBContext(options);
            await db.Database.MigrateAsync(ct);
            return db;
        }
        private async Task CreateDatabaseAsync(string databaseName, CancellationToken ct) //todo
        {
            //await using var conn = new NpgsqlConnection(_adminConnectionString);
            ////await conn.OpenAsync(ct);

            var safeName = databaseName.Replace("\"", "\"\"");

            await WithRetries(async () =>
            {
                await using var conn = new NpgsqlConnection(_adminConnectionString);
                await conn.OpenAsync(ct);

                var safeName = databaseName.Replace("\"", "\"\"");
                var sql = $"CREATE DATABASE \"{safeName}\"";

                await using var cmd = new NpgsqlCommand(sql, conn);
                try 
                { 
                    await cmd.ExecuteNonQueryAsync(ct); 
                }
                catch(PostgresException ex) when (ex.SqlState == "42P04")
                {
                    Console.WriteLine("db exists");
                }
            });

            //var sql = $@"CREATE DATABASE ""{safeName}"";";

            //await using var cmd = new NpgsqlCommand(sql, conn);

            //try
            //{
            //    await cmd.ExecuteNonQueryAsync(ct);
            //}
            //catch (PostgresException ex) when (ex.SqlState == "42P04")
            //{
            //    Console.WriteLine($"Database {databaseName} already exists.");
            //}
        }
        private async Task WithRetries(Func<Task> action, int tries = 10, int delayMs = 300)
        {
            Exception? last = null;

            for (var i = 1; i <= tries; i++)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex) when (
                    ex is NpgsqlException ||
                    ex is TimeoutException ||
                    ex is IOException ||
                    ex.InnerException is EndOfStreamException)
                {
                    last = ex;
                    await Task.Delay(delayMs);
                }
            }

            throw new InvalidOperationException("Postgres is not ready / connection is unstable.", last);
        }
    }

}       
