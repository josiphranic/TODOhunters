using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using urednistvo.Models;

namespace urednistvo.ModelsView.Textual
{
    public class StatisticsView
    {
        public DateTime To { get; set; }
        public DateTime From { get; set; }

        public Int32 NumEditions { get; set; }
        public Int32 NumAuthors { get; set; }
        public Int32 NumTexts { get; set; }

        public List<EditionView> Editions { get; set; }
        public List<AuthorView> Authors { get; set; }
        public List<TextView> Texts { get; set; }

    }
}