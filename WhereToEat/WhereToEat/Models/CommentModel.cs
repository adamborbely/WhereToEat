using System;

namespace WhereToEat.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime PostTime { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public CommentModel() { }
        public CommentModel(int id, int userId, int restaurantId, DateTime postTime, string message)
        {
            Id = id;
            UserId = userId;
            RestaurantId = restaurantId;
            PostTime = postTime;
            Message = message;
        }

        public CommentModel(CommentModel comment)
        {
            Id = comment.Id;
            UserId = comment.UserId;
            RestaurantId = comment.RestaurantId;
            PostTime = comment.PostTime;
            Message = comment.Message;
            Username = comment.Username;
        }
    }
}
