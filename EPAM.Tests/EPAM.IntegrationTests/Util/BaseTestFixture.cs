using EPAM.EF;
using EPAM.EF.Interfaces;
using EPAM.RabbitMQ.BackgroundServices;
using EPAM.Services.Backgrounds;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPAM.IntegrationTests.Util
{
    public sealed class BaseTestFixture : IDisposable
    {
        private readonly IServiceScope _serviceScope;
        private readonly SystemContext _systemContext;

        public BaseTestFixture()
        {
            var build = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration(configuration =>
                    {
                        configuration.AddJsonFile($"{Environment.CurrentDirectory}/appsettings.Testing.json");
                    });

                    builder.ConfigureServices(services =>
                    {
                        var serviceDescriptors = services
                            .Where(s => s.ImplementationType == typeof(DbUpdaterService) || s.ImplementationType == typeof(NotificationsReaderService))
                            .ToList();

                        foreach (var serviceDescriptor in serviceDescriptors)
                        {
                            services.Remove(serviceDescriptor);
                        }
                    });
                });

            Client = build.CreateClient();

            _serviceScope = build.Services.CreateScope();

            _systemContext = _serviceScope.ServiceProvider.GetRequiredService<SystemContext>();
            _systemContext.Database.EnsureDeleted();
            _systemContext.Database.EnsureCreated();

        }

        public HttpClient Client { get; }

        public ISystemContext Context => _systemContext;

        public void Dispose()
        {
            _systemContext.Database.EnsureDeleted();
            _systemContext.Dispose();
            Client.Dispose();
            _serviceScope.Dispose();
        }
    }
}
