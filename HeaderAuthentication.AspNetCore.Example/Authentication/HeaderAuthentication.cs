using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HeaderAuthentication.AspNetCore.Example.Authentication
{
    public class HeaderAuthentication : IHeaderAuthenticate
    {
        public async Task<ClaimsPrincipal> SignIn(string value)
        {
            return string.IsNullOrEmpty(value) ? null : new ClaimsPrincipal();
        }
    }
}
