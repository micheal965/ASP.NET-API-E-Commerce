using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.IServices
{
    public interface IAuthService
    {
        public Task<string> CreateWebToken(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}
