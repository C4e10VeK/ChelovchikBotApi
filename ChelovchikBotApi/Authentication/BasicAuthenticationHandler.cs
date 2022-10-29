using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using ChelovchikBotApi.Domain.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ChelovchikBotApi.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserRepository _userRepository;
    
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUserRepository userRepository) 
        : base(options, logger, encoder, clock)
    {
        _userRepository = userRepository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Authorization required");

        var authorizationHeader = Request.Headers["Authorization"].ToString();
        if (!authorizationHeader.StartsWith("Basic"))
            return AuthenticateResult.Fail("Incorrect authorization format");

        var token = authorizationHeader["Basic ".Length..].Trim();
        var encodingCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        var credentials = encodingCredentials.Split(":");
        if (credentials.Length < 2)
            return AuthenticateResult.Fail("Incorrect token");

        var users = await _userRepository.GetApiUsers();
        var user = users.FirstOrDefault(u => u.Login == credentials[0] && u.Password == credentials[1], null);
        if (user is null)
            return AuthenticateResult.Fail("");

        var claims = new[]
        {
            new Claim("name", user.Login),
            new Claim("id", Guid.NewGuid().ToString()),
            new Claim("role", "apiuser")
        };
        var identity = new ClaimsIdentity(claims, "Basic");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        
        return AuthenticateResult.Success(ticket);
    }
}