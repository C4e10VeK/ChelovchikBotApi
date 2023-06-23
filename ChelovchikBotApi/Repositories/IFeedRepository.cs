using MongoDB.Bson;
using Smile = ChelovchikBotApi.Models.Repository.Smile;
using User = ChelovchikBotApi.Models.Repository.User;

namespace ChelovchikBotApi.Repositories;

public interface IFeedRepository
{
    IEnumerable<User> GetUsersAsync();
    IEnumerable<Smile> GetSmiles();
    IEnumerable<Smile> GetSmiles(string user);
    Task<List<string?>> GetAvailableSmiles();
    Task<User?> GetUser(ObjectId id);
    Task<User?> GetUser(string name);
    Task<Smile?> GetSmile(ObjectId id);
    Task<Smile?> GetSmile(string name);
    Task AddUser(User? user);
    Task AddSmile(Smile? smile);
    Task UpdateUser(ObjectId id, User? user);
    Task<Smile?> UpdateSmile(ObjectId id, Smile? smile);
    Task<User?> AddUser(string name);
    Task<Smile?> AddSmile(string name);
}