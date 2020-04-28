using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;

namespace HeaderAuthentication.AspNetCore
{
    public class HeaderAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Header";
        public string Scheme => DefaultScheme;
        public string HeaderKey { get; set; }
        public IHeaderAuthenticate Authentication { get; set; }
    }
}
