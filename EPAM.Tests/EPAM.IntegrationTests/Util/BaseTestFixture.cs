using EPAM.EF;
using EPAM.EF.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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
                    builder.ConfigureServices(services =>
                    {
                        var serviceDescriptors = services
                        .Where(s => s.ServiceType == typeof(SystemContext) || s.ServiceType == typeof(ISystemContext) || s.ServiceType == typeof(DbContextOptions) || s.ServiceType == typeof(DbContextOptions<SystemContext>))
                        .ToList();

                        foreach (var serviceDescriptor in serviceDescriptors)
                        {
                            services.Remove(serviceDescriptor);
                        }

                        //services.AddDbContext<ISystemContext, SystemContext>(c => c.UseInMemoryDatabase("InMemoryDB")
                        //    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

                        services.AddDbContext<ISystemContext, SystemContext>(c => c.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EPAM.Intermediate.Tests;Integrated Security=True;Trusted_Connection=True;"));
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
