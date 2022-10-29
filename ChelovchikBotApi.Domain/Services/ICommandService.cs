using TwitchBot.CommandLib.Models;

namespace ChelovchikBotApi.Domain.Services;

public interface ICommandService
{
    Task<string?> Execute(string commandName, CommandContext context);
}