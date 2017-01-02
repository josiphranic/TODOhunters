using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string Type { get; set; }

        public byte[] Content { get; set; }

        public int TextId { get; set; }
        public virtual Text Text { get; set; }
    }
}