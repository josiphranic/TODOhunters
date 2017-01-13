using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsView
{
    [Flags]
    public enum TextStatus
    {
        SENT = 0x0,
        DELIVERED = 0x1,
        ACCEPTED = 0x8,
        RETURNED = 0x2,
        LECTORED = 0x16,
        DECLINED = 0x4,
        GRAPHIC = 0x64,
        CORRECTED = 0x128,
        ADDED_PICS = 0x32
    } 
}