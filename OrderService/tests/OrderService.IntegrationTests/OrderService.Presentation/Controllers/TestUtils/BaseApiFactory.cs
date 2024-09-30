using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderService.Infrastructure.Persistence.Common;
using OrderService.Infrastructure.Persistence.DbContexts;
using OrderService.Presentation.Common.Markers;
using Respawn;
using System.Data.Common;
using Testcontainers.MsSql;

namespace OrderService.IntegrationTests.OrderService.Presentation.Controllers.TestUtils
{
    public class BaseApiFactory
       : WebApplicationFactory<IApiMarker>,
       IAsyncLifetime
    {
        public HttpClient HttpClient { get; private set; } = default!;

        public ITestHarness TestHarness { get; private set; } = default!;

        private readonly MsSqlContainer _msSqlContainer =
            new MsSqlBuilder().Build();

        protected Respawner _respawner = default!;
        private DbConnection _connection = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            ConfigureDbContexts(builder);
            ConfigureMassTransit(builder);
        }

        private void ConfigureDbContexts(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<OrderDbContext>>();
                services.RemoveAll<OrderDbContext>();

                services.AddDbContext<OrderDbContext>(options =>
                {
                    options.UseSqlServer(
                            _msSqlContainer.GetConnectionString(),
                            sqlOptions =>
                            {
                                sqlOptions.MigrationsHistoryTable(
                                        DbSchemaConstants.OrderDbSchema
                                    );
                            }
                        );
                });
            });
        }

        private void ConfigureMassTransit(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IBusControl>();

                services.AddMassTransitTestHarness();
            });
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            TestHarness = this.Services.GetTestHarness();
            await TestHarness.Start();

            HttpClient = CreateClient();

            await InitializeRespawnerAsync();
        }

        private async Task InitializeRespawnerAsync()
        {
            _connection = new SqlConnection(_msSqlContainer.GetConnectionString());
            await _connection.OpenAsync();

            _respawner =
                await Respawner.CreateAsync(
                        _connection,
                        new RespawnerOptions()
                        {
                            DbAdapter = DbAdapter.SqlServer,
                            SchemasToInclude = new[]
                            {
                                DbSchemaConstants.OrderDbSchema
                            }
                        }
                    );
        }

        public new async Task DisposeAsync()
        {
            await _msSqlContainer.StopAsync();

            await TestHarness.Stop();
        }

        public async Task ResetDbAsync()
        {
            await _respawner.ResetAsync(_msSqlContainer.GetConnectionString());
        }
    }
}