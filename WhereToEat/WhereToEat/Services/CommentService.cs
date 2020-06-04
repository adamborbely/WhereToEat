using System;
using System.Collections.Generic;
using System.Data;
using WhereToEat.Controllers;
using WhereToEat.Models;

namespace WhereToEat.Services
{
    public class CommentService : ICommentService
    {
        private static CommentModel ToComment(IDataReader reader)
        {
            return new CommentModel
            {
                Id = (int)reader["comment_id"],
                UserId = (int)reader["user_id"],
                Message = (string)reader["message"],
                PostTime = (DateTime)reader["comment_time"],
                RestaurantId = (int)reader["restaurant_id"],
                Username = (string)reader["username"],
            };
        }

        private readonly IDbConnection _connection;

        public CommentService(IDbConnection connection)
        {
            _connection = connection;
        }
        public List<CommentModel> GetAllCommentsForUser(int userId)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"SELECT * FROM comments WHERE user_id = @user_id";

            var idParam = command.CreateParameter();
            idParam.ParameterName = "user_id";
            idParam.Value = userId;

            command.Parameters.Add(idParam);

            using var reader = command.ExecuteReader();
            List<CommentModel> comments = new List<CommentModel>();
            while (reader.Read())
            {
                comments.Add(ToComment(reader));
            }
            return comments;
        }

        public void AddComment(CommentModel comment, bool isApproved = false)
        {
            using var command = _connection.CreateCommand();

            var userIdParam = command.CreateParameter();
            userIdParam.ParameterName = "user_id";
            userIdParam.Value = comment.UserId;

            var restaurantIdParam = command.CreateParameter();
            restaurantIdParam.ParameterName = "restaurant_id";
            restaurantIdParam.Value = comment.RestaurantId;

            var messageParam = command.CreateParameter();
            messageParam.ParameterName = "message";
            messageParam.Value = comment.Message;


            var isApprovedParam = command.CreateParameter();
            isApprovedParam.ParameterName = "isApproved";
            isApprovedParam.Value = isApproved;

            command.CommandText = @"INSERT INTO commentsToApprove (user_id, restaurant_id, message, isApproved) 
                                    VALUES (@user_id, @restaurant_id, @message, @isApproved)";

            command.Parameters.Add(userIdParam);
            command.Parameters.Add(restaurantIdParam);
            command.Parameters.Add(messageParam);
            command.Parameters.Add(isApprovedParam);

            command.ExecuteNonQuery();
        }

        public List<CommentModel> GetAllCommentsForRestaurant(int restaurantId)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"SELECT comments.*, users.username FROM comments Join users Using (user_id) WHERE comments.restaurant_id = @restaurant_id";
            //SELECT table1.*, table2.col1, table2.col3 FROM table1 JOIN table2 USING(id)
            var idParam = command.CreateParameter();
            idParam.ParameterName = "restaurant_id";
            idParam.Value = restaurantId;

            command.Parameters.Add(idParam);

            using var reader = command.ExecuteReader();
            List<CommentModel> comments = new List<CommentModel>();
            while (reader.Read())
            {
                comments.Add(ToComment(reader));
            }
            return comments;
        }
    }
}
