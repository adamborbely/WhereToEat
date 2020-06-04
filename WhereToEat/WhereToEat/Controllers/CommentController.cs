using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhereToEat.Models;

namespace WhereToEat.Controllers
{
    public class CommentController : Controller
    {

        private readonly ICommentService _commentService;
        private readonly IRestaurantService _restaurantService;
        private readonly IStorageService _storageService;

        public CommentController(ICommentService commentService, IStorageService storageService, IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
            _commentService = commentService;
            _storageService = storageService;
        }

        [Authorize]
        public IActionResult AddComment()
        {
            var restaurantId = Request.Query["restaurantId"];
            var comment = new CommentModel { RestaurantId = Convert.ToInt32(restaurantId) };
            return View(comment);
        }

        [HttpPost]
        public IActionResult AddComment(CommentModel comment)
        {
            if (_restaurantService.GetRestaurantOwnerId(comment.RestaurantId) == comment.UserId)
            {
                _commentService.AddComment(comment, true);
            }
            else
            {
                _commentService.AddComment(comment);
            }
            return RedirectToAction("Details", "Restaurant", new { id = comment.RestaurantId });
        }

    }
}