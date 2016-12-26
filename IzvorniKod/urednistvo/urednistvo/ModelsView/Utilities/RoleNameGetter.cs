using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsView.Utilities
{
    public class RoleNameGetter
    {
        public static string getName(int i)
        {
            if ((int)Role.REGISTERED_USER == i) return "Registered user";
            if ((int)Role.AUTHOR == i) return "Author";
            if ((int)Role.LECTOR == i) return "Lector";
            if ((int)Role.GRAPHIC_EDITOR == i) return "Graphic editor";
            if ((int)Role.CORRECTOR == i) return "Corrector";
            if ((int)Role.EDITOR == i) return "Editor";
            if ((int)Role.EDITORIAL_COUNCIL_MEMBER == i) return "Editorial council member";
            return "Unknown";
        }
    }
}