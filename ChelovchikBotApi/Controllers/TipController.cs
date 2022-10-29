using ChelovchikBotApi.Domain.Models.Request;
using ChelovchikBotApi.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;
using TwitchBot.BlabLib;
using TwitchBot.BlabLib.Models;

namespace ChelovchikBotApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TipController : Controller
{
    [HttpPost(Name = "GetTip")]
    public async Task<IActionResult> GetTip([FromBody] UserNameRequest user)
    {
        if (user.Username is null)
            return BadRequest(new ResponseText {Text = "Не указано имя пользователя"});
        
        var result = await Blab.GenerateAsync(BlabType.Wisdom, $"{user.Username} вот тебе совет:");
        return Ok(result);
    }
}