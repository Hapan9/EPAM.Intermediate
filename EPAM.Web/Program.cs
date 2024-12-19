using EPAM.Cache;
using EPAM.Cache.Interfaces;
using EPAM.Cache.Models;
using EPAM.EF;
using EPAM.EF.Interfaces;
using EPAM.EF.UnitOfWork;
using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.RabbitMQ;
using EPAM.RabbitMQ.BackgroundServices;
using EPAM.RabbitMQ.Interfaces;
using EPAM.Services;
using EPAM.Services.Backgrounds;
using EPAM.Services.Interfaces;
using EPAM.Services.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddDbContext<ISystemContext, SystemContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#region Services SetUp
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();


builder.Services.AddSingleton<IRabbitMqClient, RabbitMqClient>();

builder.Services.AddHostedService<DbUpdaterService>();
builder.Services.AddHostedService<NotificationsReaderService>();
builder.Services.AddHostedService<SmsConsumerService>();
builder.Services.AddHostedService<EmailConsumerService>();
builder.Services.AddHostedService<ConsumersResultsService>();
#endregion

#region Cache SetUp
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ISystemCache, SystemCache>();
builder.Services.AddDistributedSqlServerCache(options =>
{

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    #region Create Cache DB

    var distributedOptions = builder.Configuration.GetSection("CacheOptions:DistributedOptions").Get<DistributedOptions>();

    new Process
    {
        StartInfo =
        {
            FileName = "dotnet",
            Arguments = $"sql-cache create \"{connectionString}\" {distributedOptions!.SchemaName} {distributedOptions!.TableName}"
        }
    }.Start();

    #endregion

    options.ConnectionString = connectionString;
    options.SchemaName = distributedOptions.SchemaName;
    options.TableName = distributedOptions.TableName;
});
builder.Services.AddResponseCaching();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
