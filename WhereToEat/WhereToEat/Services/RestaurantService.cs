using System;
using System.Data;
using WhereToEat.Controllers;

namespace WhereToEat.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IDbConnection _connection;

        public RestaurantService(IDbConnection connection)
        {
            _connection = connection;
        }
        public void AddRestaurant(int ownerId, string name, string city, int zipCode, string address, string image)
        {
            using var command = _connection.CreateCommand();

            var restaurantNameParam = command.CreateParameter();
            restaurantNameParam.ParameterName = "name";
            restaurantNameParam.Value = name;

            var ownerIdParam = command.CreateParameter();
            ownerIdParam.ParameterName = "owner_id";
            ownerIdParam.Value = ownerId;

            var cityParam = command.CreateParameter();
            cityParam.ParameterName = "city";
            cityParam.Value = city;

            var zipCodeParam = command.CreateParameter();
            zipCodeParam.ParameterName = "zip_code";
            zipCodeParam.Value = zipCode;

            var addressParam = command.CreateParameter();
            addressParam.ParameterName = "address";
            addressParam.Value = address;

            var imageParam = command.CreateParameter();
            imageParam.ParameterName = "restaurant_imageURL";
            imageParam.Value = (object)image ?? DBNull.Value;

            command.CommandText = @"INSERT INTO restaurants (name, city, zip_code,address, owner_id, restaurant_imageURL) 
                                    VALUES (@name, @city, @zip_code, @address, @owner_id, @restaurant_imageUrl)";

            command.Parameters.Add(restaurantNameParam);
            command.Parameters.Add(ownerIdParam);
            command.Parameters.Add(addressParam);
            command.Parameters.Add(cityParam);
            command.Parameters.Add(zipCodeParam);
            command.Parameters.Add(imageParam);
            command.ExecuteNonQuery();
        }
    }
}
