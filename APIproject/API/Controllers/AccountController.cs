using APIproject.Application.DTOs;
using APIproject.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIproject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDataProtector _protector;

        public AccountController(
            IMediator mediator,
            IDataProtectionProvider provider)
        {
            _mediator = mediator;
            _protector = provider.CreateProtector("UserRegistration");
        }

        //  Register
        // POST api/account/register

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _mediator.Send(new RegisterUserCommand(dto));

                return Ok(new { message = "OTP sent to your email. Please verify to complete registration." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "Registration failed. Please try again." });
            }
        }

        //  Verify Registration
        // POST api/account/verify-registration

        [AllowAnonymous]
        [HttpPost("verify-registration")]
        public async Task<IActionResult> VerifyRegistration([FromBody] VerifyRegistrationCommand command)
        {
            try
            {
                await _mediator.Send(command);

                return Ok(new { message = "Registration successful! You can now log in." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "Verification failed. Please try again." });
            }
        }

        //  LogIn
        // POST api/account/login

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _mediator.Send(new LoginCommand
                {
                    EmailAddress = dto.EmailAddress,
                    Password = dto.UserPassword
                });

                if (result == null)
                    return Unauthorized(new { message = "Invalid email or password." });

                // Build cookie claims — stays in controller (HTTP concern)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.UserId.ToString()),
                    new Claim(ClaimTypes.Role, result.UserRole),
                    new Claim("Role",          result.UserRole),
                    new Claim("FirstName",     result.FirstName),
                    new Claim("image",         result.UserPhoto ?? ""),
                    new Claim("email",         result.EmailAddress),
                };

                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                return Ok(new
                {
                    message = "Login successful.",
                    userId = result.UserId,
                    role = result.UserRole,
                    name = result.FirstName,
                    email = result.EmailAddress,
                    photo = result.UserPhoto
                });
            }
            catch
            {
                return StatusCode(500, new { message = "Login failed. Please try again." });
            }
        }

        //  LogOut
        // POST api/account/logout

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out successfully." });
        }

    }
}