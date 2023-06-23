using ChelovchikBotApi.Commands;
using ChelovchikBotApi.Repositories;
using TwitchBot.CommandLib;
using TwitchBot.CommandLib.Models;

namespace ChelovchikBotApi.Services;

public class CommandService : ICommandService
{
    private readonly CommandContainer _container;

    public CommandService(IFeedRepository feedRepository)
    {
        _container = new CommandContainer()
            .Add<FeedCommand>(feedRepository)
            .Add<UserCommand>(feedRepository)
            .Add<AdviceCommand>();
    }

    public async Task<string?> Execute(string commandName, CommandContext context) =>
        await _container.Execute(commandName, context);
}