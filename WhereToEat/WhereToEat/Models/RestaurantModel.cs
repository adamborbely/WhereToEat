using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Models
{
    public class RestaurantModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public int OwnerID { get; set; }

        public RestaurantModel(int id, string name, string address, double rating, int ownerId)
        {
            Id = id;
            Name = name;
            Address = address;
            Rating = rating;
            OwnerID = ownerId;
        }
    }
}
