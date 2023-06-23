using ChelovchikBotApi.Domain.Models.Repository;
using ChelovchikBotApi.Extensions;
using ChelovchikBotApi.Repositories;
using TwitchBot.CommandLib.Attributes;
using TwitchBot.CommandLib.Models;
using TwitchLib.Api;

namespace ChelovchikBotApi.Commands;

[Group(Name = "feed")]
public class FeedCommand : CommandModule
{
    private readonly IFeedRepository _feedRepository;
    private readonly Random _random;

    public FeedCommand(IFeedRepository feedRepository)
    {
        _feedRepository = feedRepository;
        _random = new Random(445662);
    }
    
    [Command(Name = "top")]
    public string? GetTop(CommandContext context)
    {
        var smiles = _feedRepository.GetSmiles()
            .Where(s => s.Size > 0)
            .OrderByDescending(s => s.Size)
            .Take(Range.EndAt(4))
            .ToList();

        var top = "Топ смайлов peepoFAT : ";
        for (var index = 0; index < smiles.Count; index++)
        {
            var smile = smiles[index];
            top += index switch
            {
                0 => "👑",
                1 => "🥈",
                2 => "🥉",
                _ => ""
            };
            top += $"{index + 1}: {smile.Name} , размер = {smile.Size:n3} см; ";
        }

        return top;
    }

    public override async Task<string?> Execute(CommandContext context)
    {
        if (context.Description is not string username)
            return "";
        
        var user = await _feedRepository.GetUser(username) ??
                   await _feedRepository.AddUser(username);
        if (user is null) return "";

        if (!context.Arguments.Any())
        {
            var availableSmiles = await _feedRepository.GetAvailableSmiles();
            return $"Можно кормить {string.Join(" ", availableSmiles)}";
        }

        var smileName = context.Arguments[0];
        var smile = await _feedRepository.GetSmile(smileName);
        if (smile is null)
        {
            var availableSmiles = await _feedRepository.GetAvailableSmiles();
            return $"Можно кормить {string.Join(" ", availableSmiles)}";
        }

        if (user.TimeToFeed > DateTime.UtcNow)
        {
            var timeToEnd = user.TimeToFeed - DateTime.UtcNow;
            return $"До следующей кормежки {timeToEnd.TotalHours:00}:{timeToEnd:mm\\:ss} peepoFAT . Жди";
        }

        smile.Size += _random.NextDouble(0.5, 0.005);
        user.TimeToFeed = DateTime.UtcNow + TimeSpan.FromMinutes(5);
        user.FeedCount++;

        if (!user.FeedSmiles.ContainsKey(smileName))
            user.FeedSmiles.Add(smile.Name, smile.Id);
        if (!smile.Users.Contains(user.Name))
            smile.Users.Add(user.Name);

        await _feedRepository.UpdateSmile(smile.Id, smile);
        await _feedRepository.UpdateUser(user.Id, user);

        return $"Ты покормил {smileName} , размер = {smile.Size:n3} см.";
    }
    
    [Command(Name = "status")]
    public async Task<string?> GetStatus(CommandContext context)
    {
        if (context.Description is not string username) return "";

        var concreteUsername = context.Arguments.Any() ? context.Arguments[0].ToLower() : username.ToLower();
        var startStr = context.Arguments.Any() ? $"{context.Arguments[0]} покормил(а)" : "Ты покормил(а)";
        var foundUser = await _feedRepository.GetUser(concreteUsername);

        if (foundUser is null)
        {
            return "Пользователь еще никого не кормил";
        }

        var smiles = _feedRepository.GetSmiles(concreteUsername).Select(s => s.Name).ToList();

        return $"{startStr} {foundUser.FeedCount} раз(а), покормленные смайлы - {string.Join(" , ", smiles)}";
    }
    
    [Command(Name = "add")]
    public async Task<string?> Add(CommandContext context)
    {
        if (context.Description is not string username) return "";

        var foundUser = await _feedRepository.GetUser(username.ToLower());

        if (foundUser is null) return "";
        if (foundUser.Permission > UserPermission.Moderator)
        {
            return "Требуются права модератора";
        }
        
        if (!context.Arguments.Any())
        {
            return "Нужно указать смайл";
        }

        var smileName = context.Arguments.First();
        var smile = await _feedRepository.AddSmile(smileName);
        
        return smile is null ? "Смайл не был добавлен" : $"Смайл {smileName} успешно добавлен";
    }
}