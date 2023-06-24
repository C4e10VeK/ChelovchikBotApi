using MongoDB.Bson;
using ChelovchikBotApi.Models.Repository;

namespace ChelovchikBotApi.Repositories;

public interface IFeedRepository
{
    IEnumerable<User> GetUsersAsync();
    IEnumerable<Smile> GetSmiles();
    IEnumerable<Smile> GetSmiles(string user);
    Task<List<string?>> GetAvailableSmiles();
    Task<User?> GetUser(ObjectId id);
    Task<User?> GetUser(string userId);
    Task<Smile?> GetSmile(ObjectId id);
    Task<Smile?> GetSmile(string name);
    Task AddUser(User? user);
    Task AddSmile(Smile? smile);
    Task UpdateUser(ObjectId id, User? user);
    Task<Smile?> UpdateSmile(ObjectId id, Smile? smile);
    Task<User?> AddUser(string userId);
    Task<Smile?> AddSmile(string name);
}