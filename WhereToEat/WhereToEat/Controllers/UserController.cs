using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhereToEat.Models;

namespace WhereToEat.Controllers
{
    public class UserController : Controller
    {
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