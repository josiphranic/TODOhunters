using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.Models.Textual
{
    public class Notification
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}