using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime PostTime { get; set; }
        public string Message { get; set; }

        public CommentModel(int id, int userId, int restaurantId, DateTime postTime, string message)
        {
            Id = id;
            UserId = userId;
            RestaurantId = restaurantId;
            PostTime = postTime;
            Message = message;
        }
    }
}
