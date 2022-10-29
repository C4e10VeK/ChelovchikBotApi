using System.Text;
using Newtonsoft.Json;
using Polly;
using TwitchBot.BlabLib.Exceptions;
using TwitchBot.BlabLib.Models;

namespace TwitchBot.BlabLib;

public class Blab
{
    private static async Task<BlabText> Get(BlabType type, string query)
    {
        using HttpClient client = new();
        var blabRequest = new BlabRequest
        {
            Query = query,
            Intro = (int)type,
            Filter = 1
        };

        var content = JsonConvert.SerializeObject(blabRequest);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://yandex.ru/lab/api/yalm/text3")
        {
            Headers =
            {
                { "Origin", "https://yandex.ru" },
                { "Referer", "https://yandex.ru/" }
            },
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        };

        using var response = await client.SendAsync(request);
        var rawResult = await response.Content.ReadAsStringAsync();
        var blabResponse = JsonConvert.DeserializeObject<BlabResponse>(rawResult);

        if (blabResponse is null || blabResponse.Error == 1)
            throw new BlabNotGeneratedException();

        return new BlabText
        {
            Qyery = blabResponse.Query,
            Text = blabResponse.Text
        };
    }

    private static async Task<BlabText> GetWhenGenerated(BlabType type, string query)
    {
        IAsyncPolicy<BlabText> retryPolicy = Policy<BlabText>
            .Handle<BlabNotGeneratedException>()
            .RetryAsync(10);

        var result = await retryPolicy.ExecuteAsync(async () => await Get(type, query));

        if (result is null)
            return new BlabText
            {
                Qyery = query,
                Text = "А нет совета"
            };
        
        return result;
    }

    public static async Task<BlabText> GenerateAsync(BlabType type, string query) =>
        await GetWhenGenerated(type, query);

    public static BlabText Generate(BlabType type, string query) => GetWhenGenerated(type, query).Result;
}