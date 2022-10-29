using Newtonsoft.Json;

namespace ChelovchikBotApi.Domain.Models.Request;

public class CommandRequest
{
    [JsonProperty("text")]
    public string? Text { get; set; }
    [JsonProperty("username")]
    public string? Username { get; set; }
}