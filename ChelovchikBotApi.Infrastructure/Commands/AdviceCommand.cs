using TwitchBot.BlabLib;
using TwitchBot.BlabLib.Models;
using TwitchBot.CommandLib.Attributes;
using TwitchBot.CommandLib.Models;

namespace ChelovchikBotApi.Infrastructure.Commands;

public class AdviceCommand : CommandModule
{
    [Command(Name = "advice")]
    public async Task<string?> GetAdvice(CommandContext context)
    {
        if (context.Description is not string username) return "";
        
        var result = await Blab.GenerateAsync(BlabType.Wisdom, "вот тебе совет:");

        return $"{result.Qyery} {result.Text}";
    }
}