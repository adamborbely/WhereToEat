using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Services
{
    public interface IPasswordHelper
    {
        public string EncryptPassword(string password);
        public bool IsValidUser(string username, string password);
    }
}
