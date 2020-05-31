using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Services
{
    public interface IUserService
    {
        public void UserRegistration(string username, string email, string pw, bool? isOwner = null);

        public int GetUserId(string email);
    }
}
