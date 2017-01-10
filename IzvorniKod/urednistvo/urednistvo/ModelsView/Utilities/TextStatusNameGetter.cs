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
            if ((int)TextStatus.SENT == i) return "Poslan";
            if ((int)TextStatus.DELIVERED == i) return "Primljen";
            if ((int)TextStatus.ACCEPTED == i) return "Prihvaćen";
            if ((int)TextStatus.RETURNED == i) return "Vraćen";
            if ((int)TextStatus.LECTORED == i) return "Lektoriran";
            if ((int)TextStatus.DECLINED == i) return "Odbijen";
            if ((int)TextStatus.GRAPHIC == i) return "Graficki ispravljen";
            if ((int)TextStatus.CORRECTED == i) return "Ispravljen od korektora";
            if ((int)TextStatus.ADDED_PICS == i) return "Dodane slike";
            return "Nepoznat";
        }
    }
}