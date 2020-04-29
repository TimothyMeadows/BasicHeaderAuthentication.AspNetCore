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
            if (string.IsNullOrEmpty(Options.HeaderKey))
                throw new ArgumentNullException("HeaderAuthenticationOptions.HeaderKey", "HeaderKey can't be null, or empty. Please read the docs and configure a header key for your application.");

            if (!Request.Headers.TryGetValue(Options.HeaderKey, out var value))
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
