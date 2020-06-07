namespace WhereToEat.Controllers
{
    public interface IRatingService
    {
        public void AddRating(int rating, int userId, int restaurantId);
        public int GetUserRatingByRestaurantId(int restauranId, int userId);
        public void DeleteRating(int userId, int restaurantId);
    }

}