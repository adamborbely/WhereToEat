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
    }
}
