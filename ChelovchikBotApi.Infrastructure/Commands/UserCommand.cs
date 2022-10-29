using ChelovchikBotApi.Domain.Repositories;
using TwitchBot.CommandLib.Attributes;
using TwitchBot.CommandLib.Models;

namespace ChelovchikBotApi.Infrastructure.Commands;

public class UserCommand : CommandModule
{
    private readonly IFeedRepository _feedRepository;

    public UserCommand(IFeedRepository feedRepository)
    {
        _feedRepository = feedRepository;
    }

    [Command(Name = "status")]
    public async Task<string?> GetStatus(CommandContext context)
    {
        if (context.Description is not string username) return "";

        var userName = context.Arguments.Any() ? context.Arguments[0].ToLower() : username;
        var startStr = context.Arguments.Any() ? $"Статус {context.Arguments[0]}" : "Твой статус";
        
        var foundUser = await _feedRepository.GetUser(userName);

        return foundUser is null
            ? "Пользователя нет в базе"
            : $"{startStr}: бан {(foundUser.IsBanned ? "есть" : "нет")}, права - {foundUser.Permission.ToString()}";
    }
}