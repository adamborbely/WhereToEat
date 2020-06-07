using System;
using System.Data;
using WhereToEat.Controllers;

namespace WhereToEat.Services
{
    public class RatingService : IRatingService
    {
        private readonly IDbConnection _connection;

        public RatingService(IDbConnection connection)
        {
            _connection = connection;
        }

        public void AddRating(int rating, int userId, int restaurantId)
        {
            using var command = _connection.CreateCommand();

            var rateParam = command.CreateParameter();
            rateParam.ParameterName = "rating";
            rateParam.Value = rating;

            var userIdParam = command.CreateParameter();
            userIdParam.ParameterName = "user_id";
            userIdParam.Value = userId;

            var restaurantIdParam = command.CreateParameter();
            restaurantIdParam.ParameterName = "restaurant_id";
            restaurantIdParam.Value = restaurantId;

            command.CommandText = @"INSERT INTO User_rating ( restaurant_id, user_id, rating) 
                                    VALUES (@restaurant_id, @user_id, @rating)";

            command.Parameters.Add(rateParam);
            command.Parameters.Add(userIdParam);
            command.Parameters.Add(restaurantIdParam);


            command.ExecuteNonQuery();
        }

        public void DeleteRating(int userId, int restaurantId)
        {
            using var command = _connection.CreateCommand();

            var userIdParam = command.CreateParameter();
            userIdParam.ParameterName = "user_id";
            userIdParam.Value = userId;

            var restaurantIdParam = command.CreateParameter();
            restaurantIdParam.ParameterName = "restaurant_id";
            restaurantIdParam.Value = restaurantId;

            command.CommandText = @"DELETE FROM user_rating WHERE restaurant_id = @restaurant_id AND user_id = @user_id";

            command.Parameters.Add(userIdParam);
            command.Parameters.Add(restaurantIdParam);

            command.ExecuteNonQuery();
        }

        public int GetUserRatingByRestaurantId(int restaurantId, int userId)
        {
            using var command = _connection.CreateCommand();

            var userIdParam = command.CreateParameter();
            userIdParam.ParameterName = "user_id";
            userIdParam.Value = userId;

            var restaurantIdParam = command.CreateParameter();
            restaurantIdParam.ParameterName = "restaurant_id";
            restaurantIdParam.Value = restaurantId;

            command.CommandText = @"SELECT rating FROM user_rating WHERE restaurant_id = @restaurant_id AND user_id = @user_id";

            command.Parameters.Add(userIdParam);
            command.Parameters.Add(restaurantIdParam);


            using var reader = command.ExecuteReader();
            reader.Read();
            int rating = Convert.ToInt32(reader["rating"]);
            return rating;
        }
    }
}
