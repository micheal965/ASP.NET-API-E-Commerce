using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.IServices;

namespace Talabat.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateWebToken(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            #region Claims For Token
            var authclaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)

            };
            var UserRoles = await userManager.GetRolesAsync(user);
            foreach (var Role in UserRoles)
                authclaims.Add(new Claim(ClaimTypes.Role, Role));
            #endregion
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: authclaims,
                expires: DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtSettings:ExpiresInDays"])),
                signingCredentials: new SigningCredentials(authkey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
