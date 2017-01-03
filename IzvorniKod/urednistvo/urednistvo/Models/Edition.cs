using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace urednistvo.Models
{
    public class Edition
    {
        public int EditionId { get; set; }
        [Required(ErrorMessage = "Tiskovina mora imati naslov")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string Title { get; set; }
        public DateTime TimeOfRelease { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
    }
}