using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhereToEat.Models;

namespace WhereToEat.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IStorageService _storageService;
        private readonly ICommentService _commentService;

        public RestaurantController(IRestaurantService restService, IStorageService storageService, ICommentService commentService)
        {
            _restaurantService = restService;
            _storageService = storageService;
            _commentService = commentService;
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

            var restaurantId = _restaurantService.GetRestaurantIdByName(restaurant.RestaurantName);

            return RedirectToAction("Details", new { id = restaurantId });
        }

        //[Authorize]
        [HttpGet]
        public IActionResult Details(int id)
        {
            var restaurant = _restaurantService.GetRestaurantById(id);
            var comments = _commentService.GetAllCommentsForRestaurant(id);
            var restaurantDetailModel = new RestaurantDetailsModel(restaurant, comments);

            return View(restaurantDetailModel);
        }

        public IActionResult Edit(int id)
        {

            var restaurant = _restaurantService.GetRestaurantById(id);
            var restaurantRegisterModel = new RestaurantRegisterModel(restaurant);
            return View(restaurantRegisterModel);
        }

        [HttpPost]
        public IActionResult Edit(RestaurantRegisterModel restaurant)
        {

            string imageFileName = restaurant.Image?.FileName;
            using Stream imageStream = restaurant.Image?.OpenReadStream();
            string image = imageFileName == null ? null : _storageService.Save(imageFileName, imageStream);


            _restaurantService.UpdateRestaurant(restaurant, image);

            var restaurantId = _restaurantService.GetRestaurantIdByName(restaurant.RestaurantName);

            return RedirectToAction("Details", new { id = restaurantId });
        }
        public IActionResult Delete(int id)
        {
            _restaurantService.DeleteRestaurant(id);

            return RedirectToAction("Details", "User");
        }
    }
}