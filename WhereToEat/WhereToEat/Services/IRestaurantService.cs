using System.Collections.Generic;
using WhereToEat.Models;

namespace WhereToEat.Controllers
{
    public interface IRestaurantService
    {
        public void AddRestaurant(int ownerId, string name, string city, int zipCode, string address, string image);
        public List<RestaurantModel> GetAll();
        public RestaurantModel GetRestaurantById(int id);
        public List<RestaurantModel> GetAllRestaurantForOwner(int id);
        public void UpdateRestaurant(RestaurantRegisterModel restaurant, string image);
        public int GetRestaurantIdByName(string restaurantName);
        public void DeleteRestaurant(int id);
        public int GetRestaurantOwnerId(int restaurantId);
    }
}