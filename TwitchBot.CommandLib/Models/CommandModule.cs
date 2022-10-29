namespace TwitchBot.CommandLib.Models;

public abstract class CommandModule : ICommandModule
{
    public virtual Task<string?> Execute(CommandContext ctx)
    {
        return Task.FromResult("");
    }
}