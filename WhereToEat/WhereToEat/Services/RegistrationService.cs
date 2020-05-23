using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IDbConnection _connection;

        public RegistrationService(IDbConnection connection)
        {
            _connection = connection;
        }


        public void UserRegistration(string username, string email, string pw)
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

            command.CommandText = @"INSERT INTO users (username, password, email) VALUES (@username, @password, @email)";
            command.Parameters.Add(usernameParam);
            command.Parameters.Add(passwordParam);
            command.Parameters.Add(emailParam);

            command.ExecuteNonQuery();
        }
    }
}
