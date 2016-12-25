using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsView.Textual
{
    public class TextView
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public string WantedSectionByAuthor { get; set; }
        public string FinalSection { get; set; }
        public bool WebPublishable { get; set; }
        public ICollection<string> Suggestions { get; set; }
        public ICollection<string> Comments { get; set; }
        public ICollection<RatingView> Ratings { get; set; }
        public TextStatus TextStatus { get; set; }
    }
}