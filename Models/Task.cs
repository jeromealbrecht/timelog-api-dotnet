using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TimeLog.Models;

public class UserTask
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public bool IsCompleted { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? CompletedAt { get; set; }
} 