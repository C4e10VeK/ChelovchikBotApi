namespace TwitchBot.CommandLib.Models;

public interface ICommandModule
{
    Task<string?> Execute(CommandContext ctx);
}