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
            if (TextStatus.SENT.Equals(i)) return "Sent";
            if (TextStatus.DELIVERED.Equals(i)) return "Delivered";
            if (TextStatus.ACCEPTED.Equals(i)) return "Accepted";
            if (TextStatus.RETURNED.Equals(i)) return "Returned";
            if (TextStatus.LECTORED.Equals(i)) return "Lectiored";
            return "Unknown";
        }
    }
}