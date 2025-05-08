using Microsoft.Extensions.Options;
using TimeLog.Models;
using TimeLog.Services;

var builder = WebApplication.CreateBuilder(args);

// Si on est sur Render (production), utilise le port d'environnement
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://0.0.0.0:" + (Environment.GetEnvironmentVariable("PORT") ?? "8080"));
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

var app = builder.Build();

// Dev only: Swagger UI et redirection HTTPS
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection(); // Redirection vers HTTPS uniquement en local
}

app.UseAuthorization();
app.MapControllers();

app.Run();