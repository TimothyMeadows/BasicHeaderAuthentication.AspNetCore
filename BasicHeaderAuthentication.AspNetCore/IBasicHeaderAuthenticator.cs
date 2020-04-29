using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicHeaderAuthentication.AspNetCore
{
    public interface IBasicHeaderAuthenticator
    {
        public Task<ClaimsPrincipal> SignIn(string value);
    }
}
