using System;
using System.Collections.Generic;

namespace urednistvo.ModelsDB
{
    public class Edition
    {
        public int Id { get; set; }
        public DateTime TimeOfRelease { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
    }
}