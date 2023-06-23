using Newtonsoft.Json;

namespace ChelovchikBotApi.Models.Response;

public class TextResponse
{
    [JsonProperty("text")]
    public string Text { get; set; }
}