using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Models
{
    public class RestaurantDetailsModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public string Address { get; set; }
        public decimal Rating { get; set; }
        public int OwnerID { get; set; }
        public string ImageURL { get; set; }
        public int UserRating { get; set; }
        public List<CommentModel> Comments { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public RestaurantDetailsModel() { }
        public RestaurantDetailsModel(RestaurantModel restaurant, List<CommentModel> comments, List<CategoryModel> categories)
        {
            Id = restaurant.Id;
            Name = restaurant.Name;
            City = restaurant.City;
            ZipCode = restaurant.ZipCode;
            Address = restaurant.Address;
            Rating = restaurant.Rating;
            OwnerID = restaurant.OwnerID;
            ImageURL = restaurant.ImageURL;
            Comments = comments.Select(x => new CommentModel(x)).ToList();
            Categories = categories.Select(x => new CategoryModel(x)).ToList();
        }

    }
}
