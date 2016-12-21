using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.Models.Textual
{
    public class Rating
    {
        public int Rate { get; set; }
        public bool WebPublishable { get; set; }
        public string SectionTitle { get; set; }
    }
}