using System;
using System.Collections.Generic;

namespace urednistvo.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}