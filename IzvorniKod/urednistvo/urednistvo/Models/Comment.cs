using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace urednistvo.Models
{
    public class Comment
    {
        [Key, ForeignKey("User"), Column(Order = 0)]
        public int UserId { get; set; }
        [Key, ForeignKey("Text"), Column(Order = 1)]
        public int TextId { get; set; }

        public string Content { get; set; }
        public DateTime Time { get; set; }

        public virtual User User { get; set; }
        public virtual Text Text { get; set; }
    }
}