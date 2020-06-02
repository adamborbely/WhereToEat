using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Models
{
    public class RestaurantListModel
    {
        public List<RestaurantModel> Restaurants { get; set; } = new List<RestaurantModel>();
    }
}
