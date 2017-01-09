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
        ACCEPTED = 0x2,
        RETURNED = 0x4,
        LECTORED = 0x8,
        DECLINED = 0x16,
        GRAPHIC = 0x32,
        CORRECTED = 0x64,
        ADDED_PICS = 0x128
    } 
}