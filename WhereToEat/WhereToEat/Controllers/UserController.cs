
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly ICommentService _commentService;
        private readonly IRestaurantService _restaurantService;

        public UserController(IUserService userService, IPasswordHelper pwHelp, ICommentService commentService, IRestaurantService restaurantService)
        {
            _userService = userService;
            _pwencrypt = pwHelp;
            _commentService = commentService;
            _restaurantService = restaurantService;
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
                        new Claim(" ", register.Username),
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

            var user = _userService.GetUser(login.Email);

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim("Id", user.ID.ToString()),
                        new Claim("Email", login.Email),
                        new Claim("Username", user.Name),
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

        public IActionResult Details()
        {
            var email = HttpContext.User.FindFirstValue("Email");
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("Id"));

            var comments = _commentService.GetAllCommentsForUser(userId);
            var restaurants = _restaurantService.GetAllRestaurantForOwner(userId);
            var user = _userService.GetUser(email);
            var pendingCommentsR = _commentService.GetPendingComments(userId);
            var pendingCommentsU = _commentService.GetUsersPendingComments(userId);

            return View(new UserDetailModel(user, restaurants, comments, pendingCommentsR, pendingCommentsU));
        }
    }
}