using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TimeLog.Models;

public class TimeLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    
    public DateTime StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public string Project { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
}