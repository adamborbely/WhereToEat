using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhereToEat.Models;

namespace WhereToEat.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restService)
        {
            _restaurantService = restService;
        }

        public IActionResult AddRestaurant()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddRestaurant(RestaurantRegisterModel restaurant)
        {
            _restaurantService.AddRestaurant(restaurant.OwnerId, restaurant.RestaurantName, restaurant.Address);

            return View();
        }
    }
}