using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Services
{
    public interface IRegistrationService
    {
        public void UserRegistration(string username, string email, string pw);
    }
}
