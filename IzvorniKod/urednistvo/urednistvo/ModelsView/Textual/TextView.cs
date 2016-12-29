using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsView.Textual
{
    public class TextView
    {
        public int TextId { get; set; }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }

        public string Username { get; set; }
        public int UserId { get; set; }

        public string WantedSectionByAuthor { get; set; }
        public string FinalSection { get; set; }
        public int? WantedSectionByAuthorId { get; set; }
        public int FinalSectionId { get; set; }

        public DateTime Time { get; set; }

        public string WebPublishable { get; set; }
        public string EditionPublishable { get; set; }

        public bool Returned { get; set; }

        public ICollection<string> Suggestions { get; set; }
        public ICollection<string> Comments { get; set; }
        public ICollection<RatingView> Ratings { get; set; }
        public string TextStatus { get; set; }
    }
}