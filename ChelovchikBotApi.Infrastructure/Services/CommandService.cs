using ChelovchikBotApi.Domain.Repositories;
using ChelovchikBotApi.Domain.Services;
using ChelovchikBotApi.Infrastructure.Commands;
using TwitchBot.CommandLib;
using TwitchBot.CommandLib.Models;

namespace ChelovchikBotApi.Infrastructure.Services;

public class CommandService : ICommandService
{
    private readonly IFeedRepository _feedRepository;
    private readonly CommandContainer _container;

    public CommandService(IFeedRepository feedRepository)
    {
        _feedRepository = feedRepository;
        _container = new CommandContainer()
            .Add<FeedCommand>(feedRepository)
            .Add<UserCommand>(feedRepository)
            .Add<AnimeCommand>(feedRepository);
    }

    public async Task<string?> Execute(string commandName, CommandContext context) =>
        await _container.Execute(commandName, context);
}