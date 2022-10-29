using Newtonsoft.Json;

namespace TwitchBot.BlabLib.Models;

public class BlabText
{
    [JsonProperty("query")]
    public string Qyery { get; set; }
    [JsonProperty("text")]
    public string Text { get; set; }
}