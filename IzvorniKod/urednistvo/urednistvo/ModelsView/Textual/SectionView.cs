using System.Collections.Generic;

namespace urednistvo.ModelsView.Textual
{
    public class SectionView
    {
        public string Title { get; set; }
        public IEnumerable<TextView> Texts { get; set; }
        // TODO IEnumerable<Image> slike
    }
}