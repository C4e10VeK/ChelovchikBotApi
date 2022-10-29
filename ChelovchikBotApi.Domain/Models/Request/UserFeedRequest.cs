using Newtonsoft.Json;

namespace ChelovchikBotApi.Domain.Models.Request;

public class UserFeedRequest
{
    [JsonProperty("username")]
    public string? Username { get; set; }
    [JsonProperty("smile_name")]
    public string? SmileName { get; set; }
}