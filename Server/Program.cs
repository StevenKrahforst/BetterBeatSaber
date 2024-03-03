using BetterBeatSaber.Server;
using BetterBeatSaber.Server.Server;
using BetterBeatSaber.Server.Server.Interfaces;
using BetterBeatSaber.Server.Services;
using BetterBeatSaber.Server.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.Configure<Server.ServerOptions>(builder.Configuration.GetSection("Server"));

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Default")!,
        optionsBuilder => optionsBuilder.EnableRetryOnFailure(3)));

builder.Services
    .AddScoped<ISteamService, SteamService>()
    .AddScoped<IPlayerService, PlayerService>()
    .AddSingleton<IServer, Server>()
    .AddHostedService(serviceProvider => serviceProvider.GetRequiredService<IServer>());

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseSwagger().UseSwaggerUI();

app.MapControllers();

#region Database Migrations

app.Logger.LogInformation("Running Migrations");

using var scope = app.Services.CreateScope();

scope.ServiceProvider
    .GetService<DatabaseContext>()!
    .Database
    .Migrate();

#endregion

app.Run();