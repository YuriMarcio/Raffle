using Raffle.Aplication.Interfaces;
using Raffle.Infrastructure.IoC;
using Raffle.Infrastructure.Repositories;
using Raffle.Infrastructure.Services;
using Raffle.Application.Interfaces;
using Raffle.Application.Services;
using Raffle.Application.Repositories;
using Raffle.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration);
// Add services to the container.


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure CORS - Allow all origins for testing
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

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
builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Enable Swagger in production for testing
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS - MUST be before UseAuthorization
app.UseCors("AllowAll");

// Configure static files serving
app.UseStaticFiles();

// Configure o pipeline
app.UseAuthorization();

app.MapControllers();

app.Run();
