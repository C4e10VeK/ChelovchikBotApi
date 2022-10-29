namespace TwitchBot.CommandLib.Models;

public interface ICommandDescription
{
    public object? Sender { get; }
    public object? Detail { get; }
}