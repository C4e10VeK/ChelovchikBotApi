using System.Net.Mime;
using ChelovchikBotApi.Domain.Models.Request;
using ChelovchikBotApi.Domain.Models.Response;
using ChelovchikBotApi.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using TwitchBot.CommandLib.Models;

namespace ChelovchikBotApi.Controllers;

[ApiController]
[Route("api/[action]")]
public class CommandController : Controller
{
    private readonly ICommandService _commandService;

    public CommandController(ICommandService commandService)
    {
        _commandService = commandService;
    }

    [HttpPost(Name = "RunCommand")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RunCommand([FromBody] CommandRequest? command)
    {
        if (command is null or {Text: null})
            return BadRequest(new TextResponse { Text = "Не указана команда"});

        var strings = command.Text.Split(" ");
        var commandName = strings[0];
        var arguments = strings[1..];

        var context = new CommandContext
        {
            Description = command.Username,
            Arguments = arguments
        };

        var result = await _commandService.Execute(commandName, context);

        return result is null or "" ? StatusCode(500, new TextResponse {Text = ""}) : Ok(new TextResponse {Text = result});
    }

}