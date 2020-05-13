using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

namespace BasicHeaderAuthentication.AspNetCore
{
    public class BasicHeaderAuthentication : AuthenticationHandler<BasicHeaderAuthenticationOptions>
    {
        private readonly IBasicHeaderAuthenticator _authenticator;

        public BasicHeaderAuthentication(IOptionsMonitor<BasicHeaderAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IServiceProvider provider) : base(options, logger, encoder, clock)
        {
            // This is so we don't create any injection errors if IBasicHeaderAuthenticator does no exist so that a more helpful message can be created.
            var scope = provider.CreateScope();
            var services = scope.ServiceProvider;

            _authenticator = services.GetService<IBasicHeaderAuthenticator>();
            if (_authenticator == null)
                throw new EntryPointNotFoundException("Missing IBasicHeaderAuthenticator dependency. Please read the docs and add a custom authentication service for header authentication.");
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headerKey = Options.HeaderKey;
            if (string.IsNullOrEmpty(headerKey))
                headerKey = "X-AuthKey";

            if (!Request.Headers.TryGetValue(headerKey, out var value))
                return Task.FromResult(AuthenticateResult.Fail("Can't find authentication header in request."));

            var claim = _authenticator.SignIn(value).Result;
            if (claim == null)
                return Task.FromResult(AuthenticateResult.Fail("Can't validate authentication header."));

            if (!claim.Identities.Any())
                claim.AddIdentity(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, value)
                }, BasicHeaderAuthenticationOptions.DefaultScheme));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claim, Options.Scheme)));
        }
    }
}
