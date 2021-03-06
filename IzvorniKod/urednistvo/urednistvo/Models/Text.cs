using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace urednistvo.Models
{
    public class Text
    {
        public int TextId { get; set; }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public int TextStatus { get; set; } //enum
        public bool WebPublishable { get; set; }
        public bool EditionPublishable { get; set; }
        public int? WantedSectionByAuthorId { get; set; }
        public string Suggestions { get; set; }

        public int FinalSectionId { get; set; }
        public virtual Section FinalSection { get; set; }

        public int UserId { get; set; }
        public virtual User Author { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Pdf> Pdf { get; set; }
    }
}