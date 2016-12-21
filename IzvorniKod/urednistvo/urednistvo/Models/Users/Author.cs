using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using urednistvo.Models.Utilities;

namespace urednistvo.Models.Users
{
    public class Author : User
    {
        public Author()
        {
            Role = Role.AUTHOR;
        }
    }
}