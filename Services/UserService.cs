using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TimeLog.Models;
using MongoDB.Bson;
using BCrypt.Net;

namespace TimeLog.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _users = database.GetCollection<User>(settings.Value.UserCollectionName);
    }

    public List<User> Get() => _users.Find(_ => true).ToList();
    public User? Get(string id) => _users.Find(u => u.Id == id).FirstOrDefault();
    public User Create(User user)
    {
        _users.InsertOne(user);
        return user;
    }

    public void Update(string id, User userIn) =>
        _users.ReplaceOne(u => u.Id == id, userIn);

    public void Remove(string id) =>
        _users.DeleteOne(u => u.Id == id);

    // Méthodes de gestion des tâches
    public UserTask? AddTask(string userId, UserTask task)
    {
        var user = _users.Find(u => u.Id == userId).FirstOrDefault();
        if (user == null) return null;

        task.Id = ObjectId.GenerateNewId().ToString();
        user.Tasks.Add(task);
        _users.ReplaceOne(u => u.Id == userId, user);
        return task;
    }

    public UserTask? UpdateTask(string userId, string taskId, UserTask updatedTask)
    {
        var user = _users.Find(u => u.Id == userId).FirstOrDefault();
        if (user == null) return null;

        var taskIndex = user.Tasks.FindIndex(t => t.Id == taskId);
        if (taskIndex == -1) return null;

        user.Tasks[taskIndex] = updatedTask;
        _users.ReplaceOne(u => u.Id == userId, user);
        return updatedTask;
    }

    public bool RemoveTask(string userId, string taskId)
    {
        var user = _users.Find(u => u.Id == userId).FirstOrDefault();
        if (user == null) return false;

        var task = user.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return false;

        user.Tasks.Remove(task);
        _users.ReplaceOne(u => u.Id == userId, user);
        return true;
    }

    public List<UserTask> GetTasks(string userId)
    {
        var user = _users.Find(u => u.Id == userId).FirstOrDefault();
        return user?.Tasks ?? new List<UserTask>();
    }

    // Méthodes de gestion de l'inscription et de la connexion
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    // Méthodes asynchrones pour AuthController
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(User user)
    {
        await _users.InsertOneAsync(user);
    }
}