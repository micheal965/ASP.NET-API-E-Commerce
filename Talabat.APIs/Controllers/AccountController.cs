using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs.Account;
using UserAddressDto = Talabat.APIs.DTOs.Account.UserAddressDto;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.IServices;
using AutoMapper;
using Talabat.APIs.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAuthService authService, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("Login")]// POST: /api/account/Login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));

            bool CheckPassword = await signInManager.UserManager.CheckPasswordAsync(user, loginDto.Password);

            if (!CheckPassword)
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateWebToken(user, userManager)
            });
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExists(registerDto.Email).Result.Value)
                return BadRequest(new ApiValidationErrorsResponse() { Errors = new string[] { "This email is already in use!" } });

            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,//michealghobriall@gmail.com
                UserName = registerDto.Email.Split('@')[0],//michealghobriall
                PhoneNumber = registerDto.PhoneNumber,
            };

            var CreationCheck = await userManager.CreateAsync(user, registerDto.Password);
            if (CreationCheck.Succeeded)
            {
                return Ok(new UserDto
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await _authService.CreateWebToken(user, userManager)
                });
            }
            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = "jwt token from database"
            });

        }
        [Authorize]
        [HttpGet("GetUserAddress")]
        public async Task<ActionResult<UserAddressDto>> GetUserAddress()
        {
            var user = await userManager.FindWithAddressByEmailAsync(User);
            var address = _mapper.Map<UserAddressDto>(user.Address);
            return Ok(address);
        }
        [Authorize]
        [HttpPut("UpdateUserAddress")]
        public async Task<ActionResult<UserAddressDto>> UpdateUserAddress(UserAddressDto UpdatedUserAddressDto)
        {
            var user = await userManager.FindWithAddressByEmailAsync(User);
            var address = _mapper.Map<UserAddressDto, Address>(UpdatedUserAddressDto);

            //for ensuring not giving a new identity or modifying it
            address.Id = user.Address.Id;

            user.Address = address;

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(address);
            else
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
        }
        [HttpGet("CheckEmailExists")]//GET :/api/Account/CheckEmailExists?email=
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }
    }

}
