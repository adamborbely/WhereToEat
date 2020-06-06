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
            };
        }
        private static PendingCommentModel ToPendingComment(IDataReader reader)
        {
            return new PendingCommentModel
            {
                Id = (int)reader["comment_id"],
                UserId = (int)reader["user_id"],
                Message = (string)reader["message"],
                PostTime = (DateTime)reader["comment_time"],
                RestaurantId = (int)reader["restaurant_id"],
                RestaurantName = (string)reader["name"],
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

            command.CommandText = @"SELECT * FROM comments WHERE user_id = @user_id ORDER BY comment_time DESC";

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

            command.CommandText = @"SELECT comments.*, users.username FROM comments Join users Using (user_id) WHERE comments.restaurant_id = @restaurant_id ORDER BY comment_time DESC";

            var idParam = command.CreateParameter();
            idParam.ParameterName = "restaurant_id";
            idParam.Value = restaurantId;

            command.Parameters.Add(idParam);

            using var reader = command.ExecuteReader();
            List<CommentModel> comments = new List<CommentModel>();
            while (reader.Read())
            {
                var commentToAdd = ToComment(reader);
                commentToAdd.Username = (string)reader["username"];
                comments.Add(commentToAdd);
            }
            return comments;
        }

        public void DeleteComment(int commentId)
        {
            using var command = _connection.CreateCommand();

            var commentIdParam = command.CreateParameter();
            commentIdParam.ParameterName = "comment_id";
            commentIdParam.Value = commentId;

            command.CommandText = @"DELETE FROM comments WHERE comment_id = @comment_id";
            command.Parameters.Add(commentIdParam);

            command.ExecuteReader();
        }

        public List<PendingCommentModel> GetPendingComments(int restaurantOwnerId)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"SELECT commentsToapprove.*, restaurants.name
                                    FROM commentsToApprove
                                    JOIN restaurants USING(restaurant_id) 
                                    WHERE commentsToApprove.restaurant_id IN 
                                    (SELECT restaurant_id FROM restaurants 
                                    WHERE owner_id = @owner_id AND isapproved = false)";

            var restaurantOwnerIdParam = command.CreateParameter();
            restaurantOwnerIdParam.ParameterName = "owner_id";
            restaurantOwnerIdParam.Value = restaurantOwnerId;

            command.Parameters.Add(restaurantOwnerIdParam);

            using var reader = command.ExecuteReader();
            List<PendingCommentModel> comments = new List<PendingCommentModel>();
            while (reader.Read())
            {
                comments.Add(ToPendingComment(reader));
            }
            return comments;
        }

        public void DismissPendingComment(int commentId)
        {
            using var command = _connection.CreateCommand();

            var commentIdParam = command.CreateParameter();
            commentIdParam.ParameterName = "comment_id";
            commentIdParam.Value = commentId;

            command.CommandText = "UPDATE commentsToApprove SET isapproved = Null WHERE comment_id = @comment_id";
            command.Parameters.Add(commentIdParam);

            command.ExecuteReader();
        }

        public void AcceptPendingComment(int commentId)
        {
            using var command = _connection.CreateCommand();

            var commentIdParam = command.CreateParameter();
            commentIdParam.ParameterName = "comment_id";
            commentIdParam.Value = commentId;

            command.CommandText = "UPDATE commentsToApprove SET isapproved = true WHERE comment_id = @comment_id";
            command.Parameters.Add(commentIdParam);

            command.ExecuteReader();
        }

        public List<PendingCommentModel> GetUsersPendingComments(int userId)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"SELECT commentsToapprove.*, restaurants.name
                                    FROM commentsToApprove
                                    JOIN restaurants USING(restaurant_id) 
                                    WHERE user_id = @user_id 
                                    AND (isapproved = false 
                                    OR isapproved is null)";

            var restaurantOwnerIdParam = command.CreateParameter();
            restaurantOwnerIdParam.ParameterName = "user_id";
            restaurantOwnerIdParam.Value = userId;

            command.Parameters.Add(restaurantOwnerIdParam);

            using var reader = command.ExecuteReader();
            List<PendingCommentModel> comments = new List<PendingCommentModel>();
            while (reader.Read())
            {
                var commentToAdd = ToPendingComment(reader);
                if (reader["isapproved"] is DBNull)
                {
                    commentToAdd.IsApproved = null;
                }
                else
                {
                    commentToAdd.IsApproved = (bool?)reader["isapproved"];
                }

                comments.Add(commentToAdd);
            }
            return comments;
        }

        public void DeletePending(int commentId)
        {
            using var command = _connection.CreateCommand();

            var commentIdParam = command.CreateParameter();
            commentIdParam.ParameterName = "comment_id";
            commentIdParam.Value = commentId;

            command.CommandText = @"DELETE FROM commentsToApprove WHERE comment_id = @comment_id";
            command.Parameters.Add(commentIdParam);

            command.ExecuteReader();
        }
    }
}

