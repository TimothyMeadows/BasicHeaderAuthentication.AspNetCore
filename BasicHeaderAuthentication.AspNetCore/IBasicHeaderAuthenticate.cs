using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicHeaderAuthentication.AspNetCore
{
    public interface IBasicHeaderAuthenticate
    {
        public Task<ClaimsPrincipal> SignIn(string value);
    }
}
