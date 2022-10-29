using Newtonsoft.Json;

namespace TwitchBot.BlabLib.Models;

internal class BlabResponse
{
    [JsonProperty("bad_query")]
    public int BadQuery { get; set; }
    [JsonProperty("query")]
    public string Query { get; set; }
    [JsonProperty("text")]
    public string Text { get; set; }
    [JsonProperty("error")]
    public int Error { get; set; }
    [JsonProperty("is_cached")]
    public int IsCached { get; set; }
    [JsonProperty("empty_zeliboba")]
    public int EmptyZeliboba { get; set; }
    [JsonProperty("intro")]
    public int Intro { get; set; }
    [JsonProperty("signature")]
    public string Signature { get; set; }
}