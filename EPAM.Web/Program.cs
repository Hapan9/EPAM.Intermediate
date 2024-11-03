using EPAM.Persistence.UnitOfWork.Interface;
using EPAM.Services;
using EPAM.Services.Interfaces;
using EPAM.Services.Profiles;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IDbConnection>(options => new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Testsql;Trusted_Connection=True;"));

builder.Services.AddScoped<IUnitOfWorkFactory, EPAM.EF.UnitOfWork.UnitOfWorkFactory>();

builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
