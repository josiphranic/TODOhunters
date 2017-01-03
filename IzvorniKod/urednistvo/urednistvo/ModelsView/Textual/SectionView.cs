using System.Collections.Generic;

namespace urednistvo.ModelsView.Textual
{
    public class SectionView
    {
        public string Title { get; set; }
        public int SectionId { get; internal set; }
        
        public int NumberOfTexts { get; set; }
    }
}