using Microsoft.AspNetCore.Http;

namespace WhereToEat.Models
{
    public class RestaurantRegisterModel
    {
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public int OwnerId { get; set; }
        public IFormFile Image { get; set; }
    }
}
