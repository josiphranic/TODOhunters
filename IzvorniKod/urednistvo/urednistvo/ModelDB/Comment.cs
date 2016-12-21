using System;

namespace urednistvo.ModelsDB
{
    public class Comment
    {
        public int PersonId { get; set; }
        public int TextId { get; set; }

        public virtual Person Person { get; set; }
        public virtual Text Text { get; set; }

        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}