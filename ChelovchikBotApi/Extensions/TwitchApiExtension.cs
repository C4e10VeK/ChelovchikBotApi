using TwitchLib.Api.Interfaces;

namespace ChelovchikBotApi.Extensions;

public static class TwitchApiExtension
{
    public static async Task<string> GetChannelId(this ITwitchAPI api, string name)
    {
        var res = await api.Helix.Users.GetUsersAsync(logins: new List<string> { name });
        if (res is null || !res.Users.Any())
            return "";

        return res.Users.First().Id;
    }
}