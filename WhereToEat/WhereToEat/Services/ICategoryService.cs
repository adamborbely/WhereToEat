using System.Collections.Generic;
using WhereToEat.Models;

namespace WhereToEat.Controllers
{
    public interface ICategoryService
    {
        public List<CategoryModel> GetCategoriesForRest(int restaurantId);
    }
}