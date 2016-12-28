using System.Collections.Generic;

namespace urednistvo.Models
{
    public class Section
    {
        public int SectionId { get; set; }
        public string Title { get; set; }

        //public int EditionId { get; set; }
        //public virtual Edition Edition { get; set; }

        public virtual ICollection<Text> Texts { get; set; }
    }
}