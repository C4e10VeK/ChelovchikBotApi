using Newtonsoft.Json;

namespace ChelovchikBotApi.Models.Response;

public class ResponseText
{
    [JsonProperty("text")]
    public string Text { get; set; }
}