using System;
using System.Collections.Generic;
using System.Data;
using WhereToEat.Controllers;
using WhereToEat.Models;

namespace WhereToEat.Services
{
    public class RestaurantService : IRestaurantService
    {
        private static RestaurantModel ToRestaurant(IDataReader reader)
        {
            return new RestaurantModel
            {
                Id = (int)reader["restaurant_id"],
                Name = (string)reader["name"],
                Address = (string)reader["address"],
                City = (string)reader["city"],
                Rating = (decimal)reader["rating"],
                OwnerID = (int)reader["owner_id"],
                ZipCode = (int)reader["zip_code"],
                ImageURL = reader["restaurant_imageURL"] as string,
            };
        }

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

        public List<RestaurantModel> GetAll()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM restaurants";

            using var reader = command.ExecuteReader();
            List<RestaurantModel> restaurants = new List<RestaurantModel>();
            while (reader.Read())
            {
                restaurants.Add(ToRestaurant(reader));
            }
            return restaurants;
        }

        public RestaurantModel GetRestaurantById(int id)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM restaurants WHERE restaurant_id = @restaurant_id";

            var idParam = command.CreateParameter();
            idParam.ParameterName = "restaurant_id";
            idParam.Value = id;

            command.Parameters.Add(idParam);
            using var reader = command.ExecuteReader();
            reader.Read();
            return ToRestaurant(reader);
        }

        public List<RestaurantModel> GetAllRestaurantForOwner(int id)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"SELECT * FROM restaurants WHERE owner_id = @owner_id";

            var idParam = command.CreateParameter();
            idParam.ParameterName = "owner_id";
            idParam.Value = id;

            command.Parameters.Add(idParam);

            using var reader = command.ExecuteReader();
            List<RestaurantModel> restaurants = new List<RestaurantModel>();
            while (reader.Read())
            {
                restaurants.Add(ToRestaurant(reader));
            }
            return restaurants;
        }

        public void UpdateRestaurant(RestaurantRegisterModel restaurant, string image)
        {
            using var command = _connection.CreateCommand();

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "name";
            nameParam.Value = restaurant.RestaurantName;

            var addressParam = command.CreateParameter();
            addressParam.ParameterName = "address";
            addressParam.Value = restaurant.Address;

            var cityParam = command.CreateParameter();
            cityParam.ParameterName = "city";
            cityParam.Value = restaurant.City;

            var zipCodeParam = command.CreateParameter();
            zipCodeParam.ParameterName = "zip_code";
            zipCodeParam.Value = restaurant.ZipCode;

            var restaurantIdParam = command.CreateParameter();
            restaurantIdParam.ParameterName = "restaurant_id";
            restaurantIdParam.Value = restaurant.Id;

            if (restaurant.Image != null)
            {
                var imageParam = command.CreateParameter();
                imageParam.ParameterName = "restaurant_imageURL";
                imageParam.Value = image;

                command.Parameters.Add(imageParam);

                command.CommandText = "UPDATE restaurants SET name = @name, address = @address, city = @city," +
                " zip_code = @zip_code, restaurant_imageUrl = @restaurant_imageUrl WHERE restaurant_id = @restaurant_id";
            }
            else
            {
                command.CommandText = "UPDATE restaurants SET name = @name, address = @address, city = @city," +
                " zip_code = @zip_code WHERE restaurant_id = @restaurant_id";
            }

            command.Parameters.Add(nameParam);
            command.Parameters.Add(addressParam);
            command.Parameters.Add(cityParam);
            command.Parameters.Add(zipCodeParam);

            command.Parameters.Add(restaurantIdParam);

            command.ExecuteNonQuery();
        }

        public int GetRestaurantIdByName(string restaurantName)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"SELECT restaurant_id FROM restaurants WHERE name = @name";

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "name";
            nameParam.Value = restaurantName;

            command.Parameters.Add(nameParam);
            using var reader = command.ExecuteReader();
            reader.Read();
            int restaurantId = Convert.ToInt32(reader["restaurant_id"]);
            return restaurantId;
        }

        public void DeleteRestaurant(int id)
        {
            using var command = _connection.CreateCommand();

            var restaurantIdParam = command.CreateParameter();
            restaurantIdParam.ParameterName = "restaurant_id";
            restaurantIdParam.Value = id;

            command.CommandText = @"DELETE FROM restaurants WHERE restaurant_id = @restaurant_id";
            command.Parameters.Add(restaurantIdParam);

            command.ExecuteReader();
        }

        public int GetRestaurantOwnerId(int restaurantId)
        {
            using var command = _connection.CreateCommand();

            var restaurantIdParam = command.CreateParameter();
            restaurantIdParam.ParameterName = "restaurant_id";
            restaurantIdParam.Value = restaurantId;

            command.CommandText = @"SELECT owner_id FROM restaurants WHERE restaurant_id = @restaurant_id";

            command.Parameters.Add(restaurantIdParam);

            using var reader = command.ExecuteReader();
            reader.Read();
            int ownerId = Convert.ToInt32(reader["owner_id"]);
            return ownerId;
        }
    }
}
