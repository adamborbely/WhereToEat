using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsOwner { get; set; }

        public UserModel() { }
        public UserModel(int id, string email, string name, bool isOwner)
        {
            ID = id;
            Email = email;
            Name = name;
            IsOwner = isOwner;
        }
    }
}
