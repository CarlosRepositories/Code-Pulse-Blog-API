using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Repository.Interface
{
    public interface ITokenRepository
    {
        string CreateToken(IdentityUser user, List<string> roles);
    }
}
