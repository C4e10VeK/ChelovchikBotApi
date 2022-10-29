using Newtonsoft.Json;

namespace ChelovchikBotApi.Models.Response;

public class UserFeedStatus
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("smiles")]
    public List<string?> Smiles { get; set; }
}