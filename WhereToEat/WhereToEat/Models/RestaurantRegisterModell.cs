using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Models
{
    public class RestaurantRegisterModel
    {
        public string RestaurantName { get; set; }
        public string Address { get; set; }

        public int OwnerId { get; set; }
    }
}
