using ChelovchikBotApi.Models.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChelovchikBotApi.Repositories;

public class FeedRepository : IFeedRepository, IUserRepository
{
    private readonly FeedDbContext _feedDbContext;
    
    public FeedRepository(FeedDbContext dbContext)
    {
        _feedDbContext = dbContext;
    }
    
    public IEnumerable<User> GetUsersAsync() => _feedDbContext.Users.Find(_ => true).ToEnumerable();

    public IEnumerable<WebApiUser> GetApiUsers() => _feedDbContext.ApiUsers.Find(_ => true).ToEnumerable();

    public IEnumerable<Smile> GetSmiles() => _feedDbContext.Smiles.Find(_ => true).ToEnumerable();

    public IEnumerable<Smile> GetSmiles(string user) =>
        _feedDbContext.Smiles.Find(s => s.Users.Contains(user)).ToEnumerable();

    public async Task<List<string?>> GetAvailableSmiles()
    {
        var list = (await _feedDbContext.Smiles.FindAsync(_ => true)).ToEnumerable();
        return list.Select(a => a.Name).ToList();
    }

    public async Task<User?> GetUser(ObjectId id)
    {
        var cursor = await _feedDbContext.Users.FindAsync(u => u.Id == id);
        return await _feedDbContext.Users.CountDocumentsAsync(u => u.Id == id) > 0 ? await cursor.SingleAsync() : null;
    }
    
    public async Task<User?> GetUser(string userId)
    {
        var cursor = await _feedDbContext.Users.FindAsync(u => u.UserId == userId);
        return await _feedDbContext.Users.CountDocumentsAsync(u => u.UserId == userId) > 0 ? await cursor.SingleAsync() : null;
    }
    
    public async Task<Smile?> GetSmile(ObjectId id)
    {
        var cursor = await _feedDbContext.Smiles.FindAsync(s => s.Id == id);
        return await _feedDbContext.Smiles.CountDocumentsAsync(s => s.Id == id) > 0 ? await cursor.SingleAsync() : null;
    }
    
    public async Task<Smile?> GetSmile(string name)
    {
        var cursor = await _feedDbContext.Smiles.FindAsync(s => s.Name == name);
        return await _feedDbContext.Smiles.CountDocumentsAsync(s => s.Name == name) > 0 ? await cursor.SingleAsync() : null;
    }

    public async Task AddUser(User? user)
    {
        if (user is null) return;
        await _feedDbContext.Users.InsertOneAsync(user);
    }

    public async Task AddSmile(Smile? smile)
    {
        if (smile is null) return;
        await _feedDbContext.Smiles.InsertOneAsync(smile);
    }

    public async Task UpdateUser(ObjectId id, User? user)
    {
        if (user is null) return;
        await _feedDbContext.Users.ReplaceOneAsync(u => u.Id == id, user);
    }

    public async Task<Smile?> UpdateSmile(ObjectId id, Smile? smile)
    {
        if (smile is null) return null;
        await _feedDbContext.Smiles.ReplaceOneAsync(s => s.Id == id, smile);
        return await GetSmile(id);
    }

    public async Task<User?> AddUser(string userId)
    {
        var user = new User
        {
            UserId = userId,
            Permission = UserPermission.User,
            IsBanned = false,
            TimeToFeed = DateTime.UtcNow
        };

        await AddUser(user);
        return await GetUser(userId);
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