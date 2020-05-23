
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WhereToEat.Models;
using WhereToEat.Services;

namespace WhereToEat.Controllers
{
    public class UserController : Controller
    {
        private readonly IRegistrationService _registerHelper;
        private readonly IPasswordHelper _pwencrypt;

        public UserController(IRegistrationService regService, IPasswordHelper pwHelp)
        {
            _registerHelper = regService;
            _pwencrypt = pwHelp;
        }

        public IActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserRegister(UserRegisterModel register)
        {
            var encryptedPW = _pwencrypt.EncryptPassword(register.Password);

            try
            {
                _registerHelper.UserRegistration(register.Username, register.Email, encryptedPW);
            }
            catch (Npgsql.PostgresException)
            {
                ModelState.AddModelError("Email", "Email Already in Use.");
                return View();
            }

            return Redirect("Login");
        }

        public IActionResult RestaurantOwnerRegister()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginAsync(LoginModel login)
        {
            if (!_pwencrypt.IsValidUser(login.Email, login.Password))
            {
                ModelState.AddModelError("Email", "Email or password not correct!");
                return View();
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login.Email) };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            { };


            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}