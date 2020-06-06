namespace WhereToEat.Controllers
{
    public interface IRatingService
    {
        public void AddRating(int rating, int userId, int restaurantId);
        public int GetUserRatingByRestaurantId(int restauranIid, int userId);
    }

}