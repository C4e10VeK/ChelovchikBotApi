namespace TwitchBot.BlabLib.Exceptions;

[Serializable]
internal class BlabNotGeneratedException : Exception
{
    public BlabNotGeneratedException() : this("Error while generate blab") { }
    public BlabNotGeneratedException(string message) : base(message) { }
}