using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Models
{
    public class RatingModel
    {
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
    }
}
