using ChelovchikBotApi.Models;
using ChelovchikBotApi.Models.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChelovchikBotApi.Repositories;

public class FeedDbContext
{
    public IMongoCollection<User> Users { get; }

    public IMongoCollection<Smile> Smiles { get; }

    public IMongoCollection<WebApiUser> ApiUsers { get; }

    public FeedDbContext(IOptions<MongoDBConfig> options)
    {
        var config = options.Value;
        var client = new MongoClient(config.ConnectionString);
        var db = client.GetDatabase(config.Database);

        Users = db.GetCollection<User>(config.UserTable);
        Smiles = db.GetCollection<Smile>(config.SmileTable);
        ApiUsers = db.GetCollection<WebApiUser>(config.ApiUserTable);
    }
}