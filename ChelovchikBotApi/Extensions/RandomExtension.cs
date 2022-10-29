namespace ChelovchikBotApi.Extensions;

public static class RandomExtension
{
    public static double NextDouble(this Random random, double max, double min)
    {
        return random.NextDouble() * (max - min) + min;
    }
}