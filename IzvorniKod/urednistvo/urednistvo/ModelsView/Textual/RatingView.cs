using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsView.Textual
{
    public class RatingView
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int TextId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public int Rate { get; set; }
        public bool WebPublishable { get; set; }
        public string SectionTitle { get; set; }
    }
}