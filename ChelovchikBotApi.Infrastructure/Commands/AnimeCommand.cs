using ChelovchikBotApi.Domain.Repositories;
using TwitchBot.CommandLib.Attributes;
using TwitchBot.CommandLib.Models;

namespace ChelovchikBotApi.Infrastructure.Commands;

[Group(Name = "anime")]
public class AnimeCommand : CommandModule
{
    private readonly IFeedRepository _feedRepository;

    public AnimeCommand(IFeedRepository feedRepository)
    {
        _feedRepository = feedRepository;
    }
    
    public override async Task<string?> Execute(CommandContext context)
    {
        if (context.Description is not string username) return "";

        var userName = context.Arguments.Any() ? context.Arguments.First().ToLower() : username.ToLower();
        var foundUser = await _feedRepository.GetUser(userName);

        if (foundUser is null)
        {
            return "Пользователя нет в базе";
        }

        var str = context.Arguments.Any() ? $"{userName}" : "Ты";
        var isAnimeStr = foundUser.IsAnime ? "анимешник D:" : "не анимешник"; 
        return $"{str} - {isAnimeStr}";
    }

    [Command(Name = "list")]
    public async Task<string?> GetList(CommandContext context)
    {
        if (context.Description is not string username) return "";

        var animes = (await _feedRepository.GetUsersAsync())
            .Where(u => u.IsAnime)
            .Aggregate("Список анимешников: ", (s, u) => s + u.Name + "; ");

        return animes;
    }
    
    [Command(Name = "add")]
    public async Task<string?> Add(CommandContext context)
    {
        if (context.Description is not string username) return "";
        if (!context.Arguments.Any()) return "Не введен пользователь";

        var userName = context.Arguments[0].ToLower();
        var foundUser = await _feedRepository.GetUser(userName) ??
                        await _feedRepository.AddUser(userName);
        
        if (foundUser is null) return "";

        if (foundUser.IsAnime)
        {
            return "Пользователь уже анимешник";
        }
        foundUser.IsAnime = true;

        await _feedRepository.UpdateUser(foundUser.Id, foundUser);
        return $"{userName} стал анимешником D:";
    }
    
    [Command(Name = "remove")]
    public async Task<string?> Remove(CommandContext context)
    {
        if (context.Description is not string username) return "";
        if (!context.Arguments.Any()) return "Не введен пользователь";

        var userName = context.Arguments[0].ToLower();
        var foundUser = await _feedRepository.GetUser(userName) ??
                        await _feedRepository.AddUser(userName);
        
        if (foundUser is null) return "";

        if (!foundUser.IsAnime)
        {
            return "Пользователь не анимешник";
        }
        foundUser.IsAnime = false;

        await _feedRepository.UpdateUser(foundUser.Id, foundUser);
        return $"{userName} больше не анимешник lizardPls";
    }
}