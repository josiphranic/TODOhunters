using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsView.Textual
{
    public class EditionView
    {
        public DateTime TimeOfRelease { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfTexts { get; set; }
        public IEnumerable<SectionView> Sections { get; set; }
    }
}