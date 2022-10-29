using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ChelovchikBotApi.Domain.Models.Repository;

public class WebApiUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public ObjectId Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}