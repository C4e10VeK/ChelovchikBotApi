using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IUserRepository = ChelovchikBotApi.Repositories.IUserRepository;
using WebApiUser = ChelovchikBotApi.Models.Repository.WebApiUser;

namespace ChelovchikBotApi.Controllers;

[ApiController, Route("api/[action]"), AllowAnonymous]
public class LoginController : Controller
{
    private readonly IUserRepository _feedRepository;
    
    public LoginController(IUserRepository feedRepository)
    {
        _feedRepository = feedRepository;
    }

    [HttpPost(Name = "Login")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] WebApiUser webApiUser)
    {
        var users = _feedRepository.GetApiUsers();

        if (!users.Any(u => u.Login == webApiUser.Login && u.Password == webApiUser.Password))
            return Unauthorized();

        var passString = $"{webApiUser.Login}:{webApiUser.Password}";
        var tokenBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(passString));

        return Json(new { token = tokenBase64 });
    }
}