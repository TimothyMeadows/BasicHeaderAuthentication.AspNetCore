using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BasicHeaderAuthentication.AspNetCore;

namespace HeaderAuthentication.AspNetCore.Example.Authentication
{
    public class BasicHeaderAuthenticate : IBasicHeaderAuthenticate
    {
        public async Task<ClaimsPrincipal> SignIn(string value)
        {
            return string.IsNullOrEmpty(value) ? null : new ClaimsPrincipal();
        }
    }
}
