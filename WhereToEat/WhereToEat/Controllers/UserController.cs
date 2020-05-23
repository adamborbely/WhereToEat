using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        public IActionResult RestaurantOwnerRegister()
        {
            return View();
        }
    }
}