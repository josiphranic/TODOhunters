using System;
using System.Collections.Generic;

namespace urednistvo.Models
{
    public class Edition
    {
        public int EditionId { get; set; }
        public string Title { get; set; }
        public DateTime TimeOfRelease { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
    }
}