using Newtonsoft.Json;

namespace ChelovchikBotApi.Domain.Models.Request;

public class UserNameRequest
{
    [JsonProperty("username")]
    public string? Username { get; set; }
}