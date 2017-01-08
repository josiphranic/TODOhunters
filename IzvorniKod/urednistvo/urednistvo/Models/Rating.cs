using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace urednistvo.Models
{
    public class Rating
    {
        [Key, ForeignKey("User"), Column(Order = 0)]
        public int UserId { get; set; }
        [Key, ForeignKey("Text"), Column(Order = 1)]
        public int TextId { get; set; }

        public bool? WebPublishable { get; set; }
        public int Rate { get; set; }
        public string SectionTitle { get; set; }

        public virtual User User { get; set; }
        public virtual Text Text { get; set; }
    }
}