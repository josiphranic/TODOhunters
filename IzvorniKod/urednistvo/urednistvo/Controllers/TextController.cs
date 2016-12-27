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
    public class TextController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        public TextView getTextView(Text text)
        {
            TextView textView = new TextView();

            textView.TextId = text.TextId;
            textView.Title = text.Title;
            textView.Subtitle = text.Subtitle;
            textView.Username = db.Users.Single(u => u.UserId == text.UserId).UserName;
            textView.UserId = text.UserId;
            textView.Time = text.Time;
            textView.Content = text.Content;
            textView.TextStatus = TextStatusNameGetter.getName(text.TextStatus);
            textView.WebPublishable = text.WebPublishable.ToString();
            textView.EditionPublishable = text.EditionPublishable.ToString();

            if (db.Sections.Count() != 0)
            {
                Section section = db.Sections.Single(s => s.SectionId == text.FinalSectionId);
                textView.FinalSection = (section == null) ? "-" : section.Title;
                section = db.Sections.Single(s => s.SectionId == text.WantedSectionByAuthorId);
                textView.WantedSectionByAuthor = (section == null) ? "-" : section.Title;
            }
            else
            {
                textView.FinalSection = "-";
                textView.WantedSectionByAuthor = "-";
            }

            return textView;
        } 

        // GET: Text
        public ActionResult Index()
        {
            List<Text> list = db.Texts.ToList();
            List<TextView> listView = new List<TextView>();

            foreach(Text t in list.ToList())
            {
                listView.Add(getTextView(t));
            }
            return View(listView);
        }

        public ActionResult ByAuthor(int id)
        {
            List<Text> list = db.Texts.Where(t => t.UserId == id).ToList();
            if (list.Count == 0)
            {
                TempData["Message"] = "No texts for this author.";
                return RedirectToAction("Index", "User");
            }
            List<TextView> listView = new List<TextView>();

            foreach (Text t in list)
            {
                listView.Add(getTextView(t));
            }
            return View(listView);
        }

        public ActionResult ForLectoring()
        {
            List<Text> list = db.Texts.Where(t => t.TextStatus == (int)TextStatus.ACCEPTED).ToList();
            if (list.Count == 0)
            {
                TempData["Message"] = "No texts for lectoring.";
                return RedirectToAction("Index", "User");
            }
            List<TextView> listView = new List<TextView>();

            foreach (Text t in list)
            {
                listView.Add(getTextView(t));
            }
            return View(listView);
        }

        // GET: Text/Details/5
        public ActionResult Details(int id)
        {
            return View(getTextView(db.Texts.Single(u => u.TextId == id)));
        }

        // GET: Text/Create
        public ActionResult Create()
        {
            if(Session["UserID"] == null)
            {
                TempData["Message"] = "You must be logged in to create a text.";
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Text/Create
        [HttpPost]
        public ActionResult Create(Text text)
        {
            if (ModelState.IsValid)
            {
                using (UrednistvoDatabase db = new UrednistvoDatabase())
                {
                    text.TextStatus = (int)TextStatus.SENT;
                    text.WebPublishable = false;
                    text.EditionPublishable = false;
                    text.FinalSectionId = -1;

                    text.UserId = Int32.Parse((String)(Session["UserID"]));;

                    text.Time = DateTime.Now;

                    db.Texts.Add(text);
                    db.SaveChanges();
                }
                ModelState.Clear();
                TempData["Message"] = "Tekst je uspjesno stvoren.";

                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View(db.Texts.Single(t => t.TextId == id));
        }

        [HttpPost]
        public ActionResult Edit(int id, Text text)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var query = from ord in db.Texts
                            where ord.TextId == id
                            select ord;

                foreach (Text t in query)
                {
                    t.Content = text.Content;

                    if ((string)Session["Role"] == "Lector")
                    {
                        t.TextStatus = (int)TextStatus.LECTORED;
                    }
                    else
                    {
                        t.TextStatus = (int)TextStatus.SENT;
                    }
                }

                db.SaveChanges();

                TempData["Message"] = "Tekst je ispravljen i ponovno poslan.";
                return RedirectToAction("Index");
            }
        }

        // GET: Text/EditEditor/5
        public ActionResult EditEditor(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var text = db.Texts.Single(d => d.TextId == id);
                return View(text);
            }
        }

        // POST: Text/EditEditor/5
        [HttpPost]
        public ActionResult EditEditor(int id, Text text, string submit)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var query = from ord in db.Texts
                            where ord.TextId == id
                            select ord;

                foreach(Text t in query)
                {
                    t.EditionPublishable = text.EditionPublishable;
                    t.WebPublishable = text.WebPublishable;
                    t.Suggestions = text.Suggestions;
                    t.FinalSectionId = text.FinalSectionId;
                    t.FinalSection = text.FinalSection;

                    if (submit == "Return")
                    {
                        NotificationController.createNotification(t, "Vas tekst mora biti doraden po uputama.");
                        t.TextStatus = (int)TextStatus.RETURNED;
                    }
                    else if (submit == "Accept")
                    {
                        NotificationController.createNotification(t, "Vas tekst je prihvacen. Ostatak informacija nalazi se u detaljima teksta.");
                        t.TextStatus = (int)TextStatus.ACCEPTED;
                    }
                }

                db.SaveChanges();

                TempData["Message"] = "Obavijest o odluci je poslana autoru teksta.";
                return RedirectToAction("Index");
            }
        }

        // GET: Text/EditMember/5
        public ActionResult EditMember(int id)
        {
            return RedirectToAction("Create/" + id, "Ratings");
        }

        // GET: Text/Delete/5
        public ActionResult Delete(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                db.Texts.Remove(db.Texts.Find(id));
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
