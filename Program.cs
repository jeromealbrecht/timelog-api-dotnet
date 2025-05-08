using Microsoft.Extensions.Options;
using TimeLog.Models;
using TimeLog.Services;

var builder = WebApplication.CreateBuilder(args);

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

app.UseAuthorization();
app.MapControllers();

app.Run();