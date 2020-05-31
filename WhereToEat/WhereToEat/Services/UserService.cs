using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Services
{
    public class UserService : IUserService
    {
        private readonly IDbConnection _connection;

        public UserService(IDbConnection connection)
        {
            _connection = connection;
        }


        public void UserRegistration(string username, string email, string pw, bool? isOwner = null)
        {
            using var command = _connection.CreateCommand();

            var usernameParam = command.CreateParameter();
            usernameParam.ParameterName = "username";
            usernameParam.Value = username;

            var emailParam = command.CreateParameter();
            emailParam.ParameterName = "email";
            emailParam.Value = email;

            var passwordParam = command.CreateParameter();
            passwordParam.ParameterName = "password";
            passwordParam.Value = pw;

            if (isOwner != null && (bool)isOwner)
            {
                var isOwnerParam = command.CreateParameter();
                isOwnerParam.ParameterName = "isOwner";
                isOwnerParam.Value = isOwner;

                command.Parameters.Add(isOwnerParam);

                command.CommandText = @"INSERT INTO users (username, password, email, isOwner) VALUES (@username, @password, @email, @isOwner)";
            }
            else
            {
                command.CommandText = @"INSERT INTO users (username, password, email) VALUES (@username, @password, @email)";
            }
            command.Parameters.Add(usernameParam);
            command.Parameters.Add(passwordParam);
            command.Parameters.Add(emailParam);
            command.ExecuteNonQuery();
        }

        public int GetUserId(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = $"SELECT user_id FROM users WHERE email LIKE '{email}'";

            using var reader = command.ExecuteReader();

            reader.Read();
            int userId = Convert.ToInt32(reader["user_id"]);
            return userId;
        }

    }
}
