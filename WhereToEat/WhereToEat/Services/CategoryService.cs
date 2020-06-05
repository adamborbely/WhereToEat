using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WhereToEat.Controllers;
using WhereToEat.Models;

namespace WhereToEat.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IDbConnection _connection;

        public CategoryService(IDbConnection connection)
        {
            _connection = connection;
        }
        public List<CategoryModel> GetCategoriesForRest(int restaurantId)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"SELECT *
                                    FROM categories as c
                                    JOIN restaurant_category as rc 
                                    ON c.category_id = rc.category_id
                                    JOIN restaurants r 
                                    ON r.restaurant_id = rc.restaurant_id
                                    WHERE  r.restaurant_id = @restaurant_id";

            var restaurantIdParam = command.CreateParameter();
            restaurantIdParam.ParameterName = "restaurant_id";
            restaurantIdParam.Value = restaurantId;

            command.Parameters.Add(restaurantIdParam);

            using var reader = command.ExecuteReader();
            List<CategoryModel> categories = new List<CategoryModel>();
            while (reader.Read())
            {
                var categorie = new CategoryModel
                {
                    Id = (int)reader["category_id"],
                    Text = (string)reader["name"]
                };
                categories.Add(categorie);
            }
            return categories;
        }
    }
}
