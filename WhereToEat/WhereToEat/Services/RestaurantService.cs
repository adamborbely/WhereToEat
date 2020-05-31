using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
        public void AddRestaurant(int ownerId, string name, string address)
        {
            using var command = _connection.CreateCommand();

            var restaurantNameParam = command.CreateParameter();
            restaurantNameParam.ParameterName = "name";
            restaurantNameParam.Value = name;

            var ownerIdParam = command.CreateParameter();
            ownerIdParam.ParameterName = "owner_id";
            ownerIdParam.Value = ownerId;

            var addressParam = command.CreateParameter();
            addressParam.ParameterName = "address";
            addressParam.Value = address;

            command.CommandText = @"INSERT INTO restaurants (name, address, owner_id) VALUES (@name, @address, @owner_id)";

            command.Parameters.Add(restaurantNameParam);
            command.Parameters.Add(ownerIdParam);
            command.Parameters.Add(addressParam);
            command.ExecuteNonQuery();
        }
    }
}
