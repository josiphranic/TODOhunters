using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using urednistvo.Models;

namespace urednistvo.ModelsView.Textual
{
    public class CommentView
    {
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public string Username { get; set; }
        public int UserId { get; set; }

        public int TextId { get; set; }
        public string TextTitle { get; set; }
    }
}