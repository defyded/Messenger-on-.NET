using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
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
                .WithWaitStrategy(Wait.ForUnixContainer().UntilExternalTcpPortIsAvailable(5432))
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
        private async Task CreateDatabaseAsync(string databaseName, CancellationToken ct)
        {
            await using var conn = new NpgsqlConnection(_adminConnectionString);
            await conn.OpenAsync(ct);
            var safeName = databaseName.Replace("\"", "\"\"");
            var sql = $@"CREATE DATABASE ""{safeName}"";";
            await using var cmd = new NpgsqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync(ct);
        }

    }

}       
