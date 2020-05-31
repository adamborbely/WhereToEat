namespace WhereToEat.Controllers
{
    public interface IRestaurantService
    {
        public void AddRestaurant(int ownerId, string name, string address);
    }
}