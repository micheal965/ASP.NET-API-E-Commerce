using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<ApplicationUser> FindWithAddressByEmailAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(us => us.Address).SingleOrDefaultAsync(us => us.Email == email);
            return user;

        }
    }
}
