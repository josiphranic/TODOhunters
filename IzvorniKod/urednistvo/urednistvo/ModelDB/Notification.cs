using System;
using System.Collections.Generic;

namespace urednistvo.ModelsDB
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public virtual ICollection<User> Persons { get; set; }
    }
}