using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ChelovchikBotApi.Models.Repository;

public class Smile
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public ObjectId Id { get; set; }
    public string? Name { get; set; }
    
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public double Size { get; set; }

    [JsonIgnore]
    public List<string> Users { get; set; } = new();
}