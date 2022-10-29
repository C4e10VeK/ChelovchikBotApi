using ChelovchikBotApi.Domain.Models;
using ChelovchikBotApi.Domain.Models.Repository;
using ChelovchikBotApi.Domain.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChelovchikBotApi.Infrastructure.Repositories;

public class FeedRepository : IFeedRepository, IUserRepository
{
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<Smile> _smiles;
    private readonly IMongoCollection<WebApiUser> _apiUsers;

    public FeedRepository(IOptions<MongoDBConfig> options)
    {
        var config = options.Value;
        var client = new MongoClient(config.ConnectionString);
        var db = client.GetDatabase(config.Database);

        _users = db.GetCollection<User>("Users");
        _smiles = db.GetCollection<Smile>(config.Table);
        _apiUsers = db.GetCollection<WebApiUser>("WebApiUsers");
    }

    public async Task<List<User>> GetUsersAsync() => await _users.Find(_ => true).ToListAsync();

    public async Task<List<WebApiUser>> GetApiUsers() => await _apiUsers.Find(_ => true).ToListAsync();

    public async Task<List<Smile>> GetSmiles() =>
        await _smiles.Find(_ => true).ToListAsync();
    
    public async Task<List<Smile>> GetSmiles(string user)
    {
        return await _smiles.Find(s => s.Users.Contains(user)).ToListAsync();
    }

    public async Task<List<string?>> GetAvailableSmiles()
    {
        var list = await _smiles.Find(_ => true).ToListAsync();
        return list.Select(a => a.Name).ToList();
    }

    public async Task<User?> GetUser(ObjectId id)
    {
        var cursor = await _users.FindAsync(u => u.Id == id);
        return await _users.CountDocumentsAsync(u => u.Id == id) > 0 ? await cursor.SingleAsync() : null;
    }
    
    public async Task<User?> GetUser(string name)
    {
        var cursor = await _users.FindAsync(u => u.Name == name);
        return await _users.CountDocumentsAsync(u => u.Name == name) > 0 ? await cursor.SingleAsync() : null;
    }
    
    public async Task<Smile?> GetSmile(ObjectId id)
    {
        var cursor = await _smiles.FindAsync(s => s.Id == id);
        return await _smiles.CountDocumentsAsync(s => s.Id == id) > 0 ? await cursor.SingleAsync() : null;
    }
    
    public async Task<Smile?> GetSmile(string name)
    {
        var cursor = await _smiles.FindAsync(s => s.Name == name);
        return await _smiles.CountDocumentsAsync(s => s.Name == name) > 0 ? await cursor.SingleAsync() : null;
    }

    public async Task AddUser(User? user)
    {
        if (user is null) return;
        await _users.InsertOneAsync(user);
    }

    public async Task AddSmile(Smile? smile)
    {
        if (smile is null) return;
        await _smiles.InsertOneAsync(smile);
    }

    public async Task UpdateUser(ObjectId id, User? user)
    {
        if (user is null) return;
        await _users.ReplaceOneAsync(u => u.Id == id, user);
    }

    public async Task<Smile?> UpdateSmile(ObjectId id, Smile? smile)
    {
        if (smile is null) return null;
        await _smiles.ReplaceOneAsync(s => s.Id == id, smile);
        return await GetSmile(id);
    }

    public async Task<User?> AddUser(string name)
    {
        var user = new User
        {
            Name = name,
            Permission = UserPermission.User,
            IsBanned = false,
            TimeToFeed = DateTime.UtcNow
        };

        await AddUser(user);
        return await GetUser(name);
    }

    public async Task<Smile?> AddSmile(string name)
    {
        var smile = new Smile
        {
            Name = name,
            Size = 0
        };

        await AddSmile(smile);
        return await GetSmile(name);
    }
}