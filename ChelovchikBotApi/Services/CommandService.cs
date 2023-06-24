using ChelovchikBotApi.Commands;
using ChelovchikBotApi.Models;
using ChelovchikBotApi.Repositories;
using Microsoft.Extensions.Options;
using TwitchBot.CommandLib;
using TwitchBot.CommandLib.Models;
using TwitchLib.Api;

namespace ChelovchikBotApi.Services;

public class CommandService : ICommandService
{
    private readonly CommandContainer _container;

    public CommandService(IOptions<TwitchAPIConfig> options, IFeedRepository feedRepository)
    {
        var twitchApi = new TwitchAPI
        {
            Settings =
            {
                ClientId = options.Value.ClientId,
                Secret = options.Value.Secret
            }
        };

        _container = new CommandContainer()
            .Add<FeedCommand>(feedRepository, twitchApi)
            .Add<UserCommand>(feedRepository)
            .Add<AdviceCommand>();
    }

    public async Task<string?> Execute(string commandName, CommandContext context) =>
        await _container.Execute(commandName, context);
}