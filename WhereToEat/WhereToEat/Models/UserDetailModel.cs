using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Models
{
    public class UserDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsOwner { get; set; }
        public List<RestaurantModel> Restaurants { get; set; }
        public List<CommentModel> Comments { get; set; }

        public UserDetailModel(UserModel user, List<RestaurantModel> restaurants, List<CommentModel> comments)
        {
            Id = user.ID;
            Name = user.Name;
            Email = user.Email;
            IsOwner = user.IsOwner;
            Restaurants = restaurants.Select(x => new RestaurantModel(x)).ToList();
            Comments = comments.Select(x => new CommentModel(x)).ToList();
        }

    }
}
