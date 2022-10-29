using Newtonsoft.Json;

namespace TwitchBot.BlabLib.Models;

internal class BlabRequest
{
    [JsonProperty("query")]
    public string Query { get; set; }
    [JsonProperty("intro")]
    public int Intro { get; set; }
    [JsonProperty("filter")]
    public int Filter { get; set; }
}