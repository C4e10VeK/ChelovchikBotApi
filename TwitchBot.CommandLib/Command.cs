using System.Reflection;
using TwitchBot.CommandLib.Models;

namespace TwitchBot.CommandLib;

internal class Command
{
    internal string? Name { get; set; }

    internal ICommandModule? Module { get; set; }

    internal MethodInfo? CommandMethod { get; set; }

    internal Command? Parent { get; set; }

    internal IReadOnlyList<Command>? Children { get; set; }

    internal async Task<string?> Execute(CommandContext context)
    {
        if (Module is null) return "";

        var res = CommandMethod?.Invoke(Module, new object?[] { context }); 
        if (res is Task<string?> task)
            return await task;

        return res as string;
    }
}