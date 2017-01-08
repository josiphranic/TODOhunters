using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsView.Utilities
{
    public class TextStatusNameGetter
    {
        public static string getName(int i)
        {
            if ((int)TextStatus.SENT == i) return "Sent";
            if ((int)TextStatus.DELIVERED == i) return "Delivered";
            if ((int)TextStatus.ACCEPTED == i) return "Accepted";
            if ((int)TextStatus.RETURNED == i) return "Returned";
            if ((int)TextStatus.LECTORED == i) return "Lectiored";
            return "Unknown";
        }
    }
}