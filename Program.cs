using Microsoft.Extensions.Options;
using TimeLog.Models;
using TimeLog.Services;

// Charger les variables d'environnement
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configuration du mode développement
var isDev = builder.Environment.IsDevelopment();

// Config Render: écouter sur le bon port
var port = isDev ? "5000" : (Environment.GetEnvironmentVariable("PORT") ?? "8080");
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Swagger + API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajoute les contrôleurs
builder.Services.AddControllers();

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Configure OpenAI
builder.Services.AddHttpClient<OpenAIService>();

// Ajouter les services
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TimeLogService>();
builder.Services.AddTransient<KeyVaultService>();
builder.Services.AddSingleton<IWhatsAppNotificationService, WhatsAppNotificationService>();

var app = builder.Build();

// Configuration de Swagger en développement
if (isDev)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();