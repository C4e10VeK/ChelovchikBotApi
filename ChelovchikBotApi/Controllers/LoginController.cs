using System.Text;
using ChelovchikBotApi.Domain.Models.Repository;
using ChelovchikBotApi.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Login([FromBody] WebApiUser webApiUser)
    {
        var users = await _feedRepository.GetApiUsers();

        if (!users.Any(u => u.Login == webApiUser.Login && u.Password == webApiUser.Password))
            return Unauthorized();

        var passString = $"{webApiUser.Login}:{webApiUser.Password}";
        var tokenBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(passString));

        return Json(new { token = tokenBase64 });
    }
}