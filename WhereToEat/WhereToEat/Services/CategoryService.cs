using System.Collections.Generic;
using System.Data;
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

        public void AddCategoryToRestaurant(int restaurantId, int categoryId)
        {
            using var command = _connection.CreateCommand();

            var restaurantIdParam = command.CreateParameter();
            restaurantIdParam.ParameterName = "restaurant_id";
            restaurantIdParam.Value = restaurantId;

            var categoryParam = command.CreateParameter();
            categoryParam.ParameterName = "category_id";
            categoryParam.Value = categoryId;

            command.CommandText = @"INSERT INTO restaurant_category
                                    (restaurant_id, category_id) VALUES (@restaurant_id, @category_id)";

            command.Parameters.Add(restaurantIdParam);
            command.Parameters.Add(categoryParam);

            command.ExecuteReader();
        }

        public List<CategoryModel> GetAllCategories()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"SELECT *
                                    FROM categories";

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

        public List<CategoryModel> GetCategoriesForRestaurant(int restaurantId)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"SELECT *
                                    FROM categories
                                    WHERE category_id NOT IN    
                                   (SELECT category_id 
                                    FROM restaurant_category
                                    WHERE restaurant_id = @restaurant_id)";

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

        public void RemoveRestaurantCategory(int restaurantId, int categoryId)
        {
            using var command = _connection.CreateCommand();

            var restaurantIdParam = command.CreateParameter();
            restaurantIdParam.ParameterName = "restaurant_id";
            restaurantIdParam.Value = restaurantId;

            var categoryParam = command.CreateParameter();
            categoryParam.ParameterName = "category_id";
            categoryParam.Value = categoryId;

            command.CommandText = @"DELETE
                                    FROM restaurant_category
                                    WHERE restaurant_id = @restaurant_id
                                    AND category_id = @category_id";

            command.Parameters.Add(restaurantIdParam);
            command.Parameters.Add(categoryParam);

            command.ExecuteReader();
        }
    }
}
