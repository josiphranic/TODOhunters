using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using urednistvo.Models.Utilities;

namespace urednistvo.Models.Users
{
    public class EditorialCouncilMember : User
    {
        public EditorialCouncilMember()
        {
            Role = Role.EDITORIAL_COUNCIL_MEMBER;
        }
    }
}