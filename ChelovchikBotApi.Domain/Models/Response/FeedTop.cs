using Newtonsoft.Json;

namespace ChelovchikBotApi.Domain.Models.Response;

public class SmileTop
{
    [JsonProperty("index")]
    public int Index { get; set; }
    [JsonProperty("index_str")]
    public string IndexStr { get; set; }
    [JsonProperty("smile")]
    public string Smile { get; set; }
    [JsonProperty("size")]
    public double Size { get; set; }
}

public class FeedTop
{
    [JsonProperty("data")]
    public List<SmileTop> Data { get; set; } = new();
}