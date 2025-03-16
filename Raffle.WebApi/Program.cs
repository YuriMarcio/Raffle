using Raffle.Aplication.Interfaces;
using Raffle.Infrastructure.IoC;
using Raffle.Infrastructure.Repositories;
using Raffle.Infrastructure.Services;
using Raffle.Application.Interfaces;
using Raffle.Application.Services;
using Raffle.Application.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Adicione os servi�os
builder.Services.AddScoped<IPrizeRepository, PrizeRepository>();
builder.Services.AddScoped<IRaffleRepository, RaffleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>(); // ADICIONADO
builder.Services.AddScoped<ITicketRepository, TicketRepository>(); // ADICIONADO

builder.Services.AddScoped<IRaffleService, RaffleService>();
builder.Services.AddScoped<IPrizeService, PrizeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure o pipeline
app.UseAuthorization();

app.MapControllers();

app.Run();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
