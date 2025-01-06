using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.IServices;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.AddScoped<IAuthService, AuthService>();

            Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

            }).AddEntityFrameworkStores<AppIdentityDbContext>();
            //JWT Authentication
            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"], // Match issuer from the JWT

                    ValidateAudience = true,
                    ValidAudience = configuration["JwtSettings:Audience"], // Match audience from the JWT

                    ValidateLifetime = true, // Ensure the token has not expired
                    ClockSkew = TimeSpan.FromMinutes(5), // Grace period to account for clock differences between client and server

                    ValidateIssuerSigningKey = true, // Ensure the signing key is correct
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]))
                };
            });
            Services.AddControllers();
            return Services;
        }
    }
}
