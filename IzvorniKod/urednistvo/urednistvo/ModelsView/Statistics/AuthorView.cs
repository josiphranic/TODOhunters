using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using urednistvo.ModelsView.Utilities;

namespace urednistvo.ModelsView
{
    public class AuthorView
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public Int32 numPublishedTexts { get; set; }
        public Int32 numDeclinedTexts { get; set; }
        public Int32 numSentTexes { get; set; }
    }
}