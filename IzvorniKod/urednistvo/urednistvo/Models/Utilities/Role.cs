using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.Models.Utilities
{
    [Flags]
    public enum Role
    {
        REGISTERED_USER = 0x0,

        EDITOR = 0x1,
        EDITORIAL_COUNCIL_MEMBER = 0x2,

        AUTHOR = 0x4,

        LECTOR = 0x8,
        GRAPHIC_EDITOR = 0x16,
        CORRECTOR = 0x32,
    }
}