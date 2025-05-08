using Microsoft.Extensions.Options;
using TimeLog.Models;
using TimeLog.Services;
using Microsoft.AspNetCore.HttpsPolicy;

var builder = WebApplication.CreateBuilder(args);

// Supprime les URLs héritées
builder.WebHost.UseSetting(WebHostDefaults.ServerUrlsKey, null);

// Configure Kestrel
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenLocalhost(5282); // HTTP
        serverOptions.ListenLocalhost(7282, listenOptions => 
        {
            listenOptions.UseHttps();
        }); // HTTPS
    });
}
else
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(8080); // HTTP pour Render
    });
}

// Swagger + API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajoute les contrôleurs
builder.Services.AddControllers();

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Ajouter les services
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TimeLogService>();
builder.Services.AddTransient<KeyVaultService>();

builder.Services.Configure<HttpsRedirectionOptions>(options =>
{
    options.HttpsPort = 5001; // Mets ici le port HTTPS que tu utilises
});

var app = builder.Build();

// Dev only: Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}
else
{
    // PAS de redirection HTTPS en production (Render gère déjà le HTTPS)
}

app.UseAuthorization();
app.MapControllers();

app.Run();
