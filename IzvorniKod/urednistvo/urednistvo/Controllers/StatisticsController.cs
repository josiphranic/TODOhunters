using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView;
using urednistvo.ModelsView.Textual;
using urednistvo.ModelsView.Utilities;

namespace urednistvo.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            if ((String)Session["Role"] == RoleNames.EDITOR || (String)Session["Role"] == RoleNames.EDITORIAL_COUNCIL_MEMBER)
            {
                TempData["Message"] = "Samo glavni urednik i članovi uredničko vijeća imaju pristup statistikama.";
                return RedirectToAction("Index", "Statistics");
            }
            return View();
        }

        //Get: Statistics/DataForRange
        public ActionResult DataForRange()
        {
            return View();
        }

        //Post: Statistics/DataForRange
        [HttpPost]
        public ActionResult DataForRange(DateTime from, DateTime to)
        {
            if (from.CompareTo(to) >= 0 || to.CompareTo(DateTime.Today.AddDays(1)) > 0)
            {
                TempData["Message"] = "Odabrani su neispravni datumi";
                return RedirectToAction("Index", "Statistics");
            }

            StatisticsView stat = new StatisticsView();
            stat.to = to.Date;
            stat.from = from.Date;

            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                List<Text> texts = db.Texts.Where(x => x.Time.CompareTo(from) >= 0 && x.Time.CompareTo(to) <= 0).ToList();
                stat.numTexts = texts.Count();
                stat.texts = texts;
            }

            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                List<Edition> editions = db.Editions.Where(x => x.TimeOfRelease.CompareTo(from) >= 0 && x.TimeOfRelease.CompareTo(to) <= 0).ToList();
                stat.numEditions = editions.Count();
                stat.editions = editions;
            }

            List<AuthorView> authors = new List<AuthorView>();
            List<User> author = getAuthors();
            foreach(User a in author)
            {
                AuthorView aView = createAuthorView(a);
                using
                 (UrednistvoDatabase db = new UrednistvoDatabase())
                {
                    aView.numPublishedTexts = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus == 0x2).ToList().Count();
                    aView.numDeclinedTexts = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus == 0x16).ToList().Count();
                    aView.numSentTexes = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus != 0x2 && x.TextStatus != 0x16).ToList().Count();
                }
                authors.Add(aView);
            }
            stat.authors = authors;
            stat.numAuthors = authors.Count();

            return View(stat);
        }

        //GET: Statistics/ByAuthors
        public ActionResult ByAuthors()
        {
            
            List<AuthorView> authors = new List<AuthorView>();
            List<User> author = getAuthors();
            
            foreach(User a in author)
            {
                AuthorView aView = createAuthorView(a);
                using
                 (UrednistvoDatabase db = new UrednistvoDatabase())
                {
                    aView.numPublishedTexts = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus == 0x2).ToList().Count();
                    aView.numDeclinedTexts = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus == 0x16).ToList().Count();
                    aView.numSentTexes = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus != 0x2 && x.TextStatus != 0x16).ToList().Count();
                }
                authors.Add(aView);
            }
            return View(authors);
        }

        public static AuthorView createAuthorView(User user)
        {
            AuthorView aView = new AuthorView();

            aView.UserId = user.UserId;
            aView.Email = user.Email;
            aView.FirstName = user.FirstName;
            aView.LastName = user.LastName;
            aView.UserName = user.UserName;
            aView.numDeclinedTexts = 0;
            aView.numPublishedTexts = 0;
            aView.numSentTexes = 0;

            return aView;
        }

        private List<User> getAuthors()
        {
            List<User> authors = new List<User>();
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                authors = db.Users.Where(x => x.Role == 2 || x.Role == 4).ToList();
            }
            return authors;
        }
    }
}