using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
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
        private readonly ICategoryService _categoryService;
        private readonly IRatingService _ratingService;

        public RestaurantController(IRestaurantService restService, IStorageService storageService, ICommentService commentService,
                                    ICategoryService categoryService, IRatingService ratingService)
        {
            _restaurantService = restService;
            _storageService = storageService;
            _commentService = commentService;
            _categoryService = categoryService;
            _ratingService = ratingService;
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
            var categories = _categoryService.GetCategoriesForRest(id); // Js doing this 
            var userRating = 0;

            try
            {
                var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("Id"));
                userRating = _ratingService.GetUserRatingByRestaurantId(id, userId);
            }
            catch (InvalidOperationException)
            {
            }

            var restaurantDetailModel = new RestaurantDetailsModel(restaurant, comments, categories);
            restaurantDetailModel.UserRating = userRating;

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
        public IActionResult AddRating(int rating, int restaurantId)
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("Id"));
            _ratingService.AddRating(rating, userId, restaurantId);

            return RedirectToAction("Details", new { id = restaurantId });
        }
        public IActionResult DeleteRating(int restaurantId)
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("Id"));
            _ratingService.DeleteRating(userId, restaurantId);

            return RedirectToAction("Details", new { id = restaurantId });
        }

        [HttpPost]
        public List<CategoryModel> GetCategories()
        {
            var restaurantId = Convert.ToInt32(Request.Form["restaurantId"]);
            var categories = _categoryService.GetCategoriesForRest(restaurantId);
            return categories;
        }

        [Authorize]
        public void RemoveCategory()
        {
            var restaurantId = Convert.ToInt32(Request.Form["restaurantId"]);
            var category = Convert.ToInt32(Request.Form["categoryId"]);
            _categoryService.RemoveRestaurantCategory(restaurantId, category);
        }

        public List<CategoryModel> GetCategoriesForRestaurant()
        {
            var restaurantId = Convert.ToInt32(Request.Form["restaurantId"]);

            var categories = _categoryService.GetCategoriesForRestaurant(restaurantId);
            return categories;
        }

        [Authorize]
        public void AddCategoryToRestaurant()
        {
            var restaurantId = Convert.ToInt32(Request.Form["restaurantId"]);
            var category = Convert.ToInt32(Request.Form["categoryId"]);

            _categoryService.AddCategoryToRestaurant(restaurantId, category);
        }
    }
}