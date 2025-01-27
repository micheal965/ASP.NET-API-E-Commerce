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
using Microsoft.IdentityModel.Tokens;
using Talabat.Service;
using Talabat.Core.Entities.Email;
using System.Net;
using Talabat.APIs.DTOs.Order;
using Talabat.Repository;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.WebUtilities;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Text;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AccountController(IEmailService emailService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAuthService authService, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _authService = authService;
            _emailService = emailService;
            _mapper = mapper;
        }

        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
        [HttpPost("Login")]// POST: /api/account/Login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));

            if (!user.EmailConfirmed)
            {
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Please confirm your email before logging in."));
            }
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

        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiValidationErrorsResponse>(StatusCodes.Status400BadRequest)]
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
                // Send a verification email (this could be handled by a separate service)
                await _emailService.SendVerificationEmailAsync(user);
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Registration successful! Please check your email for a verification link to activate your account."));
            }
            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
        }

        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.Replace("Bearer ", string.Empty);

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = token,
            });

        }
        [ProducesResponseType<UserAddressDto>(StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("GetUserAddress")]
        public async Task<ActionResult<UserAddressDto>> GetUserAddress()
        {
            var user = await userManager.FindWithAddressByEmailAsync(User);
            var address = _mapper.Map<UserAddressDto>(user.Address);
            if (address == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Address not found"));
            return Ok(address);
        }

        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPut("UpdateUserAddress")]
        public async Task<ActionResult<Address>> UpdateUserAddress(UserAddressDto UpdatedUserAddressDto)
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
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        [HttpGet("CheckEmailExists")]//GET :/api/Account/CheckEmailExists?email=
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Email cannot be null or empty."));

            bool result = await userManager.FindByEmailAsync(email) is not null;

            if (result)
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Email exists."));
            else
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Email not found."));
        }
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPost("Logout")]
        public async Task<ActionResult> Logout([FromHeader] string authorization)
        {
            if (authorization.IsNullOrEmpty() || !authorization.StartsWith("Bearer "))
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Invalid Authorization header"));

            var token = authorization.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Token is missing or invalid"));
            //added to blacklist
            await _authService.AddToBlacklistAsync(token);
            return Ok(new ApiResponse(StatusCodes.Status200OK, "Logged out successfully"));
        }
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [HttpPost("SendEmail")]
        public async Task<ActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.ToEmail) || string.IsNullOrEmpty(request.Subject) || string.IsNullOrEmpty(request.Body))
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid email request."));
            }

            await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);
            return Ok(new ApiResponse(StatusCodes.Status200OK, "Email sent successfully."));
        }
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [HttpGet("VerifyEmail")]
        public async Task<ActionResult> VerifyEmail(string token, string email)
        {
            // Find the user by email
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid Email"));
            // Confirm the user's email
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            var normalToken = Encoding.UTF8.GetString(decodedToken);
            var result = await userManager.ConfirmEmailAsync(user, normalToken);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid or expired token."));

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateWebToken(user, userManager)
            });
        }
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestDto model)
        {
            // Get the currently logged in user
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "User not found."));

            // Check if the old password is correct
            var checkPasswordResult = await userManager.CheckPasswordAsync(user, model.oldPassword);
            if (!checkPasswordResult)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Old password is incorrect."));

            // Change the password
            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
            if (!changePasswordResult.Succeeded)
                return BadRequest(new ApiValidationErrorsResponse() { Errors = changePasswordResult.Errors.Select(e => e.Description) });

            // Return success message
            return Ok(new ApiResponse(StatusCodes.Status200OK, "Password changed successfully."));
        }
    }

}


