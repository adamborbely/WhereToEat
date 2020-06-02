using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereToEat.Models;

namespace WhereToEat.Services
{
    public interface IUserService
    {
        public void UserRegistration(string username, string email, string pw, bool? isOwner = null);

        public int GetUserId(string email);

        public UserModel GetUser(string email);
    }
}
