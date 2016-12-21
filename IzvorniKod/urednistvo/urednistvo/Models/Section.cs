using System.Collections.Generic;

namespace urednistvo.Models
{
    public class Section
    {
        public string Title { get; set; }
        public IEnumerable<Text> Texts { get; set; }
        // TODO IEnumerable<Image> slike
    }
}