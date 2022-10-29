using ChelovchikBotApi.Domain.Models.Repository;
using MongoDB.Bson;

namespace ChelovchikBotApi.Domain.Repositories;

public interface IFeedRepository
{
    Task<List<User>> GetUsersAsync();
    Task<List<Smile>> GetSmiles();
    Task<List<Smile>> GetSmiles(string user);
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