using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Micheal Ghobrial",
                    Email = "michealghobriall@gmail.com",
                    UserName = "Micheal.Ghobrial",
                    PhoneNumber = "01201605049",
                };
                await userManager.CreateAsync(user, "P@$$w0rd");
            }
        }
    }
}
