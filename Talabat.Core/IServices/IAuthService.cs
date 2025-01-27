using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.IServices
{
    public interface IAuthService
    {
        public Task<string> CreateWebToken(ApplicationUser user, UserManager<ApplicationUser> userManager);
        public Task<bool> IsTokenBlacklistedAsync(string token);
        public Task AddToBlacklistAsync(string token);
    }
}
