using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using IUserRepository = ChelovchikBotApi.Repositories.IUserRepository;

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

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Authorization required"));

        var authorizationHeader = Request.Headers["Authorization"].ToString();
        if (!authorizationHeader.StartsWith("Basic"))
            return Task.FromResult(AuthenticateResult.Fail("Incorrect authorization format"));

        var token = authorizationHeader["Basic ".Length..].Trim();
        var encodingCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        var credentials = encodingCredentials.Split(":");
        if (credentials.Length < 2)
            return Task.FromResult(AuthenticateResult.Fail("Incorrect token"));

        var users = _userRepository.GetApiUsers();
        var user = users.FirstOrDefault(u => u.Login == credentials[0] && u.Password == credentials[1], null);
        if (user is null)
            return Task.FromResult(AuthenticateResult.Fail("User undefined"));

        var claims = new[]
        {
            new Claim("name", user.Login),
            new Claim("id", Guid.NewGuid().ToString()),
            new Claim("role", "apiuser")
        };
        var identity = new ClaimsIdentity(claims, "Basic");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}