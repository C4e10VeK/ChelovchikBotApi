using System.Reflection;
using TwitchBot.CommandLib.Attributes;
using TwitchBot.CommandLib.Models;

namespace TwitchBot.CommandLib;

internal static class CommandModuleExtension
{
    internal static Command? GetGroupCommand(this ICommandModule module)
    {
        var isGroup = module.GetType().GetCustomAttribute<GroupAttribute>() is not null;
        if (!isGroup) return null;
        var groupAttrib = module.GetType().GetCustomAttributes<GroupAttribute>(false).First();
        var defaultMethod = module.GetType().GetMethod(nameof(module.Execute));
        return new Command
        {
            Name = groupAttrib.Name ?? throw new ArgumentNullException(),
            Module = module,
            CommandMethod = defaultMethod
        };
    }

    internal static List<Command> GetCommands(this ICommandModule module, Command? commandGroup)
    {
        return module.GetType().GetMethods()
            .Where(m => m.GetCustomAttribute<CommandAttribute>(false) is not null)
            .Select(m =>
            {
                var commandAttrib = m.GetCustomAttribute<CommandAttribute>();
                return new Command
                {
                    Name = commandAttrib?.Name ?? throw new ArgumentNullException(),
                    Module = module,
                    CommandMethod = m,
                    Parent = commandGroup
                };
            }).ToList();
    }
}