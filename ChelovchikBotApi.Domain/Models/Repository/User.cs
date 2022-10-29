using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChelovchikBotApi.Domain.Models.Repository;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public string? Name { get; set; }
    
    [BsonDefaultValue(UserPermission.User)]
    public UserPermission Permission { get; set; }
    
    [BsonDefaultValue(false)]
    public bool IsBanned { get; set; }
    
    [BsonDefaultValue(false)]
    public bool IsAnime { get; set; }
    
    [BsonDefaultValue(false)]
    public bool IsAuto { get; set; }
    
    public DateTime TimeToFeed { get; set; } = DateTime.UtcNow;

    [BsonDefaultValue(0)]
    public int FeedCount { get; set; }

    public Dictionary<string, ObjectId> FeedSmiles { get; set; } = new();
}