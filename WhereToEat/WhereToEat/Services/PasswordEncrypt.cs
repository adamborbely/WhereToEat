using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WhereToEat.Services
{
    public class PasswordEncrypt : IPasswordHelper
    {
        private readonly IDbConnection _connection;

        public PasswordEncrypt(IDbConnection connection)
        {
            _connection = connection;
        }
        public string EncryptPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);//Create salt - injection for further security

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);//Create the Rfc2898DeriveBytes and get the hash value:
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];  // combine the salt and pw
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);//  Turn the combined salt+hash into a string for storage
            return savedPasswordHash;
        }

        public bool IsValidUser(string email, string password)
        {
            try
            {
                string savedPassword = GetUserPassword(email);// get the pw from database

                byte[] hashBytes = Convert.FromBase64String(savedPassword);
                /* Get the salt */
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
                /* Compute the hash on the password the user entered */
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);
                /* Compare the results */
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public string GetUserPassword(string email)
        {
            using var command = _connection.CreateCommand();

            var emailParam = command.CreateParameter();
            emailParam.ParameterName = "email";
            emailParam.Value = email;

            command.CommandText = $"SELECT password FROM users WHERE email = @email";
            command.Parameters.Add(emailParam);

            using var reader = command.ExecuteReader();
            reader.Read();
            return (string)reader["password"];
        }
    }
}
