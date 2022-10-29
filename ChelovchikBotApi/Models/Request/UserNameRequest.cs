using Newtonsoft.Json;

namespace ChelovchikBotApi.Models;

public class UserNameRequest
{
    [JsonProperty("username")]
    public string? Username { get; set; }
}