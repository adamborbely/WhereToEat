
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
        private readonly IUserService _userService;
        private readonly IPasswordHelper _pwencrypt;

        public UserController(IUserService userService, IPasswordHelper pwHelp)
        {
            _userService = userService;
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
                _userService.UserRegistration(register.Username, register.Email, encryptedPW);
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

        [HttpPost]
        public async Task<ActionResult> RestaurantOwnerRegister(UserRegisterModel register)
        {
            var encryptedPW = _pwencrypt.EncryptPassword(register.Password);

            try
            {
                _userService.UserRegistration(register.Username, register.Email, encryptedPW, true);
            }
            catch (Npgsql.PostgresException)
            {
                ModelState.AddModelError("Email", "Email Already in Use.");
                return View();
            }

            var userId = _userService.GetUserId(register.Email);

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim("Id", userId.ToString()),
                        new Claim("Email",  register.Email),
                    }, CookieAuthenticationDefaults.AuthenticationScheme)),
                    new AuthenticationProperties());

            return Redirect("../Restaurant/AddRestaurant");
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

            var userId = _userService.GetUserId(login.Email);

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim("Id", userId.ToString()),
                        new Claim("Email", login.Email),
                    }, CookieAuthenticationDefaults.AuthenticationScheme)),
                    new AuthenticationProperties());

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