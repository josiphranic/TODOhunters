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
            if (!((String)Session["Role"] == RoleNames.EDITOR || (String)Session["Role"] == RoleNames.EDITORIAL_COUNCIL_MEMBER))
            {
                TempData["Message"] = "Samo glavni urednik i članovi uredničkog vijeća imaju pristup statistikama.";
                return RedirectToAction("Index", "Home");
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
            if (from == null || to == null)
            {
                TempData["Message"] = "Vremenski preriod za za prikaz statistika mora biti odabran.";
                return RedirectToAction("Index", "Statistics");
            }

            StatisticsView stat = new StatisticsView();
            stat.To = to.Date;
            stat.From = from.Date;

            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                List<Text> texts = db.Texts.Where(x => x.Time.CompareTo(from) >= 0 && x.Time.CompareTo(to) <= 0).ToList();
                stat.NumTexts = texts.Count();
                stat.Texts = AddTexts(texts);
            }

            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                List<Edition> editions = db.Editions.Where(x => x.TimeOfRelease.CompareTo(from) >= 0 && x.TimeOfRelease.CompareTo(to) <= 0).ToList();
                stat.NumEditions = editions.Count();
                stat.Editions = AddEditions(editions);
            }

            List<AuthorView> authors = new List<AuthorView>();
            List<User> author = GetAuthors();
            foreach (User a in author)
            {
                AuthorView aView = CreateAuthorView(a);
                using
                 (UrednistvoDatabase db = new UrednistvoDatabase())
                {
                    aView.NumPublishedTexts = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus == 0x2 && x.Time.CompareTo(from) >= 0 && x.Time.CompareTo(to) <= 0).ToList().Count();
                    aView.NumDeclinedTexts = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus == 0x16 && x.Time.CompareTo(from) >= 0 && x.Time.CompareTo(to) <= 0).ToList().Count();
                    aView.NumSentTexes = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus != 0x2 && x.TextStatus != 0x16 && x.Time.CompareTo(from) >= 0 && x.Time.CompareTo(to) <= 0).ToList().Count();
                }
                authors.Add(aView);
            }
            stat.Authors = authors;
            stat.NumAuthors = authors.Count();

            return View(stat);
        }

        //GET: Statistics/ByAuthors
        public ActionResult ByAuthors()
        {

            List<AuthorView> authors = new List<AuthorView>();
            List<User> author = GetAuthors();

            foreach (User a in author)
            {
                AuthorView aView = CreateAuthorView(a);
                using
                 (UrednistvoDatabase db = new UrednistvoDatabase())
                {
                    aView.NumPublishedTexts = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus == 0x2).ToList().Count();
                    aView.NumDeclinedTexts = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus == 0x16).ToList().Count();
                    aView.NumSentTexes = db.Texts.Where(x => x.Author.UserId == a.UserId && x.TextStatus != 0x2 && x.TextStatus != 0x16).ToList().Count();
                }
                authors.Add(aView);
            }
            return View(authors);
        }

        public static AuthorView CreateAuthorView(User user)
        {
            AuthorView aView = new AuthorView();

            aView.UserId = user.UserId;
            aView.Email = user.Email;
            aView.FirstName = user.FirstName;
            aView.LastName = user.LastName;
            aView.UserName = user.UserName;
            aView.NumDeclinedTexts = 0;
            aView.NumPublishedTexts = 0;
            aView.NumSentTexes = 0;

            return aView;
        }

        private List<EditionView> AddEditions(List<Edition> editions)
        {
            List<EditionView> eView = new List<EditionView>();
            foreach (Edition e in editions)
            {
                eView.Add(EditionsController.createEditionView(e));
            }
            return eView;
        }

        private List<TextView> AddTexts(List<Text> texts)
        {
            List<TextView> tView = new List<TextView>();
            foreach (Text t in texts)
            {
                tView.Add(TextController.getTextView(t));
            }
            return tView;
        }

        private List<User> GetAuthors()
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