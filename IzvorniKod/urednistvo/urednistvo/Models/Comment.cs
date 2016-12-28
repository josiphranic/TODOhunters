using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace urednistvo.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int TextId { get; set; }    

        public string Content { get; set; }
        public DateTime Time { get; set; }

        public virtual User User { get; set; }
        public virtual Text Text { get; set; }
    }
}