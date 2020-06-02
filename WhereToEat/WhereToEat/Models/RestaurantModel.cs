namespace WhereToEat.Models
{
    public class RestaurantModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public string Address { get; set; }
        public decimal Rating { get; set; }
        public int OwnerID { get; set; }
        public string ImageURL { get; set; }
        public RestaurantModel() { }
        public RestaurantModel(int id, string name, string city, int zipCode, string address, decimal rating, int ownerId, string img)
        {
            Id = id;
            Name = name;
            City = city;
            ZipCode = zipCode;
            Address = address;
            Rating = rating;
            OwnerID = ownerId;
            ImageURL = img;
        }

        public RestaurantModel(RestaurantModel restaurant)
        {
            Id = restaurant.Id;
            Name = restaurant.Name;
            City = restaurant.City;
            Address = restaurant.Address;
            Rating = restaurant.Rating;
            ZipCode = restaurant.ZipCode;
            ImageURL = restaurant.ImageURL;
        }
    }
}
