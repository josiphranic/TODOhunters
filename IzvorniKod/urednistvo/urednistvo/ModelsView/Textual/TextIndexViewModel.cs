using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsView.Textual
{
    public class TextIndexViewModel
    {
        public int TextID { get; set; }

        public string Title { get; set; }
        public string Subtitle { get; set; }

        public string Username { get; set; }
        public int UserID { get; set; }

        public string FinalSection { get; set; }
        public DateTime Time { get; set; }
    }
}