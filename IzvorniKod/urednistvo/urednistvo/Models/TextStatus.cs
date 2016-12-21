using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.Models
{
    [Flags]
    public enum TextStatus
    {
        SENT = 0x0,
        DELIVERED = 0x1,
        ACCEPTED = 0x2,
        RETURNED = 0x4,
        LECTORED = 0x8
    }
}