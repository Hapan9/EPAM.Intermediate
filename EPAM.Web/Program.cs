using EPAM.Persistence.UnitOfWork;
using EPAM.Persistence.UnitOfWork.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddScoped<IDbConnection>(options => new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Testsql;Trusted_Connection=True;"));

if (builder.Configuration.GetValue<bool>("UseEntityFramework"))
{
    builder.Services.AddScoped<IUnitOfWorkFactory, EPAM.EF.UnitOfWork.UnitOfWorkFactory>();
}
else
{
    builder.Services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
}

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
