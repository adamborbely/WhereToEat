using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Models
{
    public class PendingCommentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime PostTime { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public string RestaurantName { get; set; }
        public bool? IsApproved { get; set; }
        public PendingCommentModel() { }
        public PendingCommentModel(int id, int userId, int restaurantId, DateTime postTime, string message, string restaurantName, bool? isApproved)
        {
            Id = id;
            UserId = userId;
            RestaurantId = restaurantId;
            PostTime = postTime;
            Message = message;
            RestaurantName = restaurantName;
            IsApproved = isApproved;
        }

        public PendingCommentModel(PendingCommentModel comment)
        {
            Id = comment.Id;
            UserId = comment.UserId;
            RestaurantId = comment.RestaurantId;
            PostTime = comment.PostTime;
            Message = comment.Message;
            Username = comment.Username;
            RestaurantName = comment.RestaurantName;
            IsApproved = comment.IsApproved;
        }
    }
}
