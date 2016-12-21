using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.Models
{
    public class Text
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }
        public Section WantedSectionByAuthor { get; set; }
        public Section FinalSection { get; set; }
        public bool WebPublishable { get; set; }
        public IEnumerable<string> Suggestions { get; set; }
        public IEnumerable<string> Comments { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
        public TextStatus TextStatus { get; set; }
    }
}