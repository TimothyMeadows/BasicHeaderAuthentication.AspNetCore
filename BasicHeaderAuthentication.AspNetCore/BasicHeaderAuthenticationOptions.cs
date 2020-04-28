using Microsoft.AspNetCore.Authentication;

namespace BasicHeaderAuthentication.AspNetCore
{
    public class BasicHeaderAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Header";
        public string Scheme => DefaultScheme;
        public string HeaderKey { get; set; }
        public IBasicHeaderAuthenticate Authenticate { get; set; }
    }
}
