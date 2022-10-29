using TwitchBot.CommandLib.Models;

namespace TwitchBot.CommandLib;

public class CommandContainer
{
    private readonly List<Command> _commands;

    public CommandContainer()
    {
        _commands = new List<Command>();
    }

    public CommandContainer Add<T>(params object[] args) where T : class, ICommandModule
    {
        var module = (T?)Activator.CreateInstance(typeof(T), args);
        if (module is null) return this;
        var commandGroup = module.GetGroupCommand();

        var commands = module.GetCommands(commandGroup);

        if (commandGroup is not null)
        {
            commandGroup.Children = commands;
            _commands.Add(commandGroup);
            return this;
        }
        
        _commands.AddRange(commands);

        return this;
    }

    public CommandContainer Add<T>() where T : class, ICommandModule, new()
    {
        var module = new T();
        var commandGroup = module.GetGroupCommand();

        var commands = module.GetCommands(commandGroup);

        if (commandGroup is not null)
        {
            commandGroup.Children = commands;
            _commands.Add(commandGroup);
            return this;
        }
        
        _commands.AddRange(commands);

        return this;
    }

    public async Task<string?> Execute(string command, CommandContext context)
    {
        var cmd = _commands.First(c => c.Name == command);
        if (!(cmd.Children ?? Array.Empty<Command>()).Any())
            return await _commands
                .First(c => c.Name == command && c.Children is null && c.Parent is null)
                .Execute(context);
        
        var subCommand = context.Arguments.Any() ? context.Arguments[0] : "";

        if (!cmd.Children?.Any(c => c.Name == subCommand) ?? true)
        {
            return await cmd.Execute(context);
        }

        context.Arguments = context.Arguments.ToArray()[1..];

        var subCommandObject = cmd.Children?.First(c => c.Name == subCommand);
        if (subCommandObject is null)
            return "";

        return await subCommandObject.Execute(context);
    }
}
