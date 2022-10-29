namespace TwitchBot.CommandLib.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class GroupAttribute : Attribute
{
    public string? Name { get; set; }
}