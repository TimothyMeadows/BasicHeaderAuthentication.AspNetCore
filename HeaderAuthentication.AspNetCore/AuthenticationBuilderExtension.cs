using System;
using Microsoft.AspNetCore.Authentication;

namespace HeaderAuthentication.AspNetCore
{
    public static class AuthenticationBuilderExtension
    {
        public static AuthenticationBuilder AddHeaderAuthentication(this AuthenticationBuilder builder, Action<HeaderAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<HeaderAuthenticationOptions, HeaderAuthentication>(HeaderAuthenticationOptions.DefaultScheme, configureOptions);
        }
    }
}
