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

        }
    }
}
