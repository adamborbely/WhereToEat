using System.IO;
using Microsoft.AspNetCore.Mvc;
using WhereToEat.Models;

namespace WhereToEat.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IStorageService _storageService;

        public RestaurantController(IRestaurantService restService, IStorageService storageService)
        {
            _restaurantService = restService;
            _storageService = storageService;
        }

        public IActionResult AddRestaurant()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddRestaurant(RestaurantRegisterModel restaurant)
        {
            string imageFileName = restaurant.Image?.FileName;
            using Stream imageStream = restaurant.Image?.OpenReadStream();
            string image = imageFileName == null ? null : _storageService.Save(imageFileName, imageStream);

            _restaurantService.AddRestaurant(restaurant.OwnerId, restaurant.RestaurantName,
                restaurant.City, restaurant.ZipCode, restaurant.Address, image);

            return View();
        }
    }
}