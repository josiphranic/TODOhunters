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
        public SectionView WantedSectionByAuthor { get; set; }
        public SectionView FinalSection { get; set; }
        public bool WebPublishable { get; set; }
        public IEnumerable<string> Suggestions { get; set; }
        public IEnumerable<string> Comments { get; set; }
        public IEnumerable<RatingView> Ratings { get; set; }
        public TextStatus TextStatus { get; set; }
    }
}