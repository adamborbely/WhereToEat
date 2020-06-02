using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
