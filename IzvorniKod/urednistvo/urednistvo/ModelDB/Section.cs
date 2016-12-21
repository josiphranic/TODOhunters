using System.Collections.Generic;

namespace urednistvo.ModelsDB
{
    public class Section
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int EditionId { get; set; }

        public virtual Edition Edition { get; set; }
        public virtual ICollection<Text> Texts { get; set; }

    }
}