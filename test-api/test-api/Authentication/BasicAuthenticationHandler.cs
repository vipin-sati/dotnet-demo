using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using test_api.Interfaces;

namespace test_api.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IUserService userService) 
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endPoint = Context.GetEndpoint();

            if (endPoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            if (Request.Headers.ContainsKey("UserName") && Request.Headers.ContainsKey("Password"))
                return AuthenticateResult.Fail("Missing required headers.");

            User user = null;

            try
            {
                var userName = AuthenticationHeaderValue.Parse(Request.Headers["UserName"]).Parameter;
                var password = AuthenticationHeaderValue.Parse(Request.Headers["Password"]).Parameter;
                user = await _userService.Authenticate(userName, password);
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("some error occured.");
            }

            if(user is null)
                return AuthenticateResult.Fail("UserName or Password is not correct.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principle = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principle, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
