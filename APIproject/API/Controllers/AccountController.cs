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
    public class AccountController : Controller
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

        // Register 

        [HttpGet]
        public IActionResult Register() => View();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return View(dto);

                await _mediator.Send(new RegisterUserCommand(dto));

                return RedirectToAction("VerifyRegistration", new { email = dto.EmailAddress });
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(dto);
            }
            catch
            {
                ModelState.AddModelError("", "Registration failed. Please try again.");
                return View(dto);
            }
        }

        // Verify Registration

        [HttpGet]
        public IActionResult VerifyRegistration(string email)
            => View(new VerifyRegistrationCommand { EmailAddress = email });

        [HttpPost]
        public async Task<IActionResult> VerifyRegistration(VerifyRegistrationCommand command)
        {
            try
            {
                await _mediator.Send(command);

                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(command);
            }
            catch
            {
                ModelState.AddModelError("", "Verification failed. Please try again.");
                return View(command);
            }
        }

        // Login

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var result = await _mediator.Send(new LoginCommand
                {
                    EmailAddress = dto.EmailAddress,
                    Password = dto.UserPassword
                });

                if (result == null)
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                    return View(dto);
                }

                // Claims — HTTP concern, stays in controller
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.UserId.ToString()),
                    new Claim(ClaimTypes.Role, result.UserRole),
                    new Claim("Role",          result.UserRole),
                    new Claim("FirstName",      result.FirstName),
                    new Claim("image",         result.UserPhoto ?? ""),
                    new Claim("email",         result.EmailAddress),
                };

                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                return RedirectToAction("Dashboard");
            }
            catch
            {
                ModelState.AddModelError("", "Login failed. Please try again.");
                return View(dto);
            }
        }

        // Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        
    }
}