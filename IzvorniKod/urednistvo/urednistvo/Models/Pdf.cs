using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.Models
{
    public class Pdf
    {
        public int PdfId { get; set; }
        public string PdfName { get; set; }

        public int TextId { get; set; }
        public virtual Text Text { get; set; } 

    }
}