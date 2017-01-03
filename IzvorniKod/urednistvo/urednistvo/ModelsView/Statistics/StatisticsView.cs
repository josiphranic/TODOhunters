﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using urednistvo.Models;

namespace urednistvo.ModelsView.Textual
{
    public class StatisticsView
    {
        public DateTime to { get; set; }
        public DateTime from { get; set; }

        public Int32 numEditions { get; set; }
        public Int32 numAuthors { get; set; }
        public Int32 numTexts { get; set; }

        public List<Edition> editions { get; set; }
        public List<AuthorView> authors { get; set; }
        public List<Text> texts { get; set; }

    }
}