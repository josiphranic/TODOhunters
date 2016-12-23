using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsView.Textual
{
    public class RatingView
    {
        public int Rate { get; set; }
        public bool WebPublishable { get; set; }
        public string SectionTitle { get; set; }
    }
}