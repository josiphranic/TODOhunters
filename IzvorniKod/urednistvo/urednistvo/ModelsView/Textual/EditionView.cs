using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsView.Textual
{
    public class EditionView
    {
        public int EditionId { get; set; }
        public string Title { get; set; }
        public DateTime TimeOfRelease { get; set; }
        public DateTime StartTime { get; set; }
        public int NumberOfTexts { get; set; }
        public IEnumerable<SectionView> Sections { get; set; }
    }
}