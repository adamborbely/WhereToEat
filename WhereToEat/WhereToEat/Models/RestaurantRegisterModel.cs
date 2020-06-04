using Microsoft.AspNetCore.Http;

namespace WhereToEat.Models
{
    public class RestaurantRegisterModel
    {
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public int OwnerId { get; set; }
        public IFormFile Image { get; set; }

        public RestaurantRegisterModel() { }
        public RestaurantRegisterModel(RestaurantModel restaurant)
        {
            OwnerId = restaurant.OwnerID;
            Id = restaurant.Id;
            RestaurantName = restaurant.Name;
            Address = restaurant.Address;
            City = restaurant.City;
            ZipCode = restaurant.ZipCode;
        }
    }
}
