﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urednistvo.Models
{
    public class Edition
    {
        public DateTime TimeOfRelease { get; set; }
        public IEnumerable<Section> Sections { get; set; }
    }
}