using Newtonsoft.Json;

namespace ChelovchikBotApi.Models;

public class UserFeedRequest
{
    [JsonProperty("username")]
    public string? Username { get; set; }
    [JsonProperty("smile_name")]
    public string? SmileName { get; set; }
}