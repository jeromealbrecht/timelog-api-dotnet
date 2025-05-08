using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TimeLog.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastLogin { get; set; }

    // Liste des tâches associées à l'utilisateur
    public List<UserTask> Tasks { get; set; } = new List<UserTask>();
}