using EPAM.EF;
using EPAM.EF.Interfaces;
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
                });

            Client = build.CreateClient();

            _serviceScope = build.Services.CreateScope();

            _systemContext = _serviceScope.ServiceProvider.GetRequiredService<SystemContext>();

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
