using System;
using Microsoft.AspNetCore.Authentication;

namespace BasicHeaderAuthentication.AspNetCore
{
    public static class AuthenticationBuilderExtension
    {
        public static AuthenticationBuilder AddBasicHeaderAuthentication(this AuthenticationBuilder builder, Action<BasicHeaderAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<BasicHeaderAuthenticationOptions, BasicHeaderAuthentication>(BasicHeaderAuthenticationOptions.DefaultScheme, configureOptions);
        }
    }
}
