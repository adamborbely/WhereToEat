using System.Collections.Generic;
using WhereToEat.Models;

namespace WhereToEat.Controllers
{
    public interface ICategoryService
    {
        public List<CategoryModel> GetCategoriesForRest(int restaurantId);
        public List<CategoryModel> GetCategoriesForRestaurant(int restaurantId);
        public void RemoveRestaurantCategory(int restaurantId, int categoryId);
        public List<CategoryModel> GetAllCategories();
        public void AddCategoryToRestaurant(int restaurantId, int categoryID);
    }
}