using ChelovchikBotApi.Domain.Models.Repository;
using ChelovchikBotApi.Domain.Models.Request;
using ChelovchikBotApi.Domain.Models.Response;
using ChelovchikBotApi.Domain.Repositories;
using ChelovchikBotApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ChelovchikBotApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class FeedController : Controller
{
    private readonly IFeedRepository _feedRepository;
    private readonly Random _random = new();

    public FeedController(IFeedRepository repository)
    {
        _feedRepository = repository;
    }
    
    [HttpGet(Name = "GetTop")]
    public async Task<IActionResult> GetTop()
    {
        var smiles = (await _feedRepository.GetSmiles())
            .Where(s => s.Size > 0)
            .OrderByDescending(s => s.Size)
            .Take(Range.EndAt(4))
            .ToList();
        var feedTop = new FeedTop();
        for (var index = 0; index < smiles.Count; index++)
        {
            var smile = smiles[index];
            var indexStr = index switch
            {
                0 => "👑",
                1 => "🥈",
                2 => "🥉",
                _ => ""
            };
            feedTop.Data.Add(new SmileTop
            {
                Index = index + 1,
                IndexStr = indexStr,
                Smile = smile.Name ?? string.Empty,
                Size = smile.Size
            });
        }
        
        return Json(feedTop);
    }

    [HttpPut(Name = "Feed")]
    public async Task<IActionResult> Feed([FromBody] UserFeedRequest? userFeedRequest)
    {
        if (userFeedRequest is null or {Username: null, SmileName: null})
            return BadRequest(new ResponseText{ Text = "Не указан пользователь или смайл"});
        
        var user = await _feedRepository.GetUser(userFeedRequest.Username) ??
                   await _feedRepository.AddUser(userFeedRequest.Username);
        if (user is null)
        {
            return NotFound(new ResponseText{ Text = "Пользователь не найден" });
        }
        
        var smile = await _feedRepository.GetSmile(userFeedRequest.SmileName);
        if (smile is null)
        {
            var availableSmiles = await _feedRepository.GetAvailableSmiles();
            return NotFound(new ResponseText{ Text = $"Можно кормить {string.Join(" ", availableSmiles)}"});
        }

        if (user.TimeToFeed > DateTime.UtcNow)
        {
            var timeToEnd = user.TimeToFeed - DateTime.UtcNow;

            return Ok(new ResponseText
                {Text = $"До следующей кормежки {timeToEnd.TotalHours:00}:{timeToEnd:mm\\:ss} peepoFAT . Жди"});
        }

        smile.Size += _random.NextDouble(0.5, 0.005);
        user.TimeToFeed = DateTime.UtcNow + TimeSpan.FromMinutes(5);
        user.FeedCount++;

        if (!user.FeedSmiles.ContainsKey(userFeedRequest.SmileName))
            user.FeedSmiles.Add(smile.Name, smile.Id);
        if (!smile.Users.Contains(user.Name))
            smile.Users.Add(user.Name);
        
        await _feedRepository.UpdateSmile(smile.Id, smile);
        await _feedRepository.UpdateUser(user.Id, user);
        
        return Json(new ResponseText{ Text = $"Ты покормил {userFeedRequest.SmileName} , размер = {smile.Size:n3} см." });
    }

    [HttpGet(Name = "Feed")]
    public async Task<IActionResult> Feed()
    {
        var availableSmiles = await _feedRepository.GetAvailableSmiles();
        return Json(new ResponseText{ Text = $"Можно кормить {string.Join(" ", availableSmiles)}"});
    }

    [HttpPost(Name = "GetFeedStatus")]
    public async Task<IActionResult> GetFeedStatus([FromBody] UserNameRequest? username)
    {
        if (username is null or {Username: null})
            return BadRequest(new ResponseText { Text = "не указано имя пользователя" });
        
        var foundUser = await _feedRepository.GetUser(username.Username);
        
        if (foundUser is null)
        {
            return NotFound(new ResponseText{ Text = "Пользователь еще никого не кормил" });
        }

        var smiles = (await _feedRepository.GetSmiles(username.Username)).Select(s => s.Name).ToList();

        return Json(new UserFeedStatus
        {
            Username = username.Username,
            Smiles = smiles
        });
    }

    [HttpPost(Name = "Add")]
    public async Task<IActionResult> Add([FromBody] UserFeedRequest? userFeedRequest)
    {
        if (userFeedRequest is null or {Username: null, SmileName: null})
            return BadRequest(new ResponseText{ Text = "Не указан пользователь или смайл"});
        
        var foundUser = await _feedRepository.GetUser(userFeedRequest.Username);

        if (userFeedRequest.SmileName is null)
        {
            return BadRequest(new ResponseText{ Text = "название смайла не указано" });
        }

        if (foundUser is null) 
            return NotFound(new ResponseText{ Text = "Пользователь не найден" });
        if (foundUser.Permission > UserPermission.Moderator)
        {
            return Ok(new ResponseText{ Text = "Требуются права модератора" });
        }

        var smile = await _feedRepository.AddSmile(userFeedRequest.SmileName);
        if (smile is null) 
            return BadRequest();

        return Json(new ResponseText{ Text = $"Смайл {userFeedRequest.SmileName} успешно добавлен" });
    }
}