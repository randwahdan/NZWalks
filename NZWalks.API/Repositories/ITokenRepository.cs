using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
