using Newtonsoft.Json;

namespace ChelovchikBotApi.Domain.Models.Response;

public class ResponseText
{
    [JsonProperty("text")]
    public string Text { get; set; }
}