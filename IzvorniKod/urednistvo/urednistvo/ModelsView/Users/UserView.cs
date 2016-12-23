using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using urednistvo.ModelsView.Utilities;

namespace urednistvo.ModelsView
{
    public class UserView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public UserView()
        {
            Role = Role.REGISTERED_USER;
        }
    }
}