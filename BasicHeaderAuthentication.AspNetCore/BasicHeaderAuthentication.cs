using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

namespace BasicHeaderAuthentication.AspNetCore
{
    public class BasicHeaderAuthentication : AuthenticationHandler<BasicHeaderAuthenticationOptions>
    {
        public BasicHeaderAuthentication(IOptionsMonitor<BasicHeaderAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            // ...
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (string.IsNullOrEmpty(Options.HeaderKey))
                throw new ArgumentNullException("Options.HeaderKey", "HeaderAuthenticationOptions.HeaderKey can't be null, or empty. Please read the docs and configure a header key for your application.");

            if (Options.Authenticate == null)
                throw new ArgumentNullException("Options.Authenticate", "HeaderAuthenticationOptions.Authenticate can't be null. Please read the docs and configure a authentication method for your application.");

            if (!Request.Headers.TryGetValue(Options.HeaderKey, out var value))
                return Task.FromResult(AuthenticateResult.Fail("Cannot find authentication header in request."));

            var claim = Options.Authenticate.SignIn(value).Result;
            if (claim == null)
                return Task.FromResult(AuthenticateResult.Fail("Cannot validate authentication header."));

            if (!claim.Identities.Any())
                claim.AddIdentity(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, value)
                }, BasicHeaderAuthenticationOptions.DefaultScheme));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claim, Options.Scheme)));
        }
    }
}
