using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HeaderAuthentication.AspNetCore
{
    public interface IHeaderAuthenticate
    {
        public Task<ClaimsPrincipal> SignIn(string value);
    }
}
