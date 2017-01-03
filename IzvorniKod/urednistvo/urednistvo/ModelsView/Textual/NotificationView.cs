using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using urednistvo.Models;

namespace urednistvo.ModelsView.Textual
{
    public class NotificationView
    {
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public int UserId { get; set; }
        public string ForUser { get; set; }
    }
}