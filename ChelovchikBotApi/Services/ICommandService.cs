using TwitchBot.CommandLib.Models;

namespace ChelovchikBotApi.Services;

public interface ICommandService
{
    Task<string?> Execute(string commandName, CommandContext context);
}