using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TimeLog.Models;

namespace TimeLog.Services;

public class TimeLogService
{
    private readonly IMongoCollection<Models.TimeLog> _timeLogs;

    public TimeLogService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _timeLogs = database.GetCollection<Models.TimeLog>("TimeLogs");
    }

    public List<Models.TimeLog> Get() => _timeLogs.Find(_ => true).ToList();
    public Models.TimeLog Get(string id) => _timeLogs.Find(t => t.Id == id).FirstOrDefault();
    public Models.TimeLog Create(Models.TimeLog log)
    {
        _timeLogs.InsertOne(log);
        return log;
    }

    public void Update(string id, Models.TimeLog logIn) =>
        _timeLogs.ReplaceOne(t => t.Id == id, logIn);

    public void Remove(string id) =>
        _timeLogs.DeleteOne(t => t.Id == id);
}